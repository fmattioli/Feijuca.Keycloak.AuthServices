using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Errors;
using TokenManager.Domain.Interfaces;
using TokenManager.Infra.Data.Models;

namespace TokenManager.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenCredentials _tokenCredentials;
        private readonly HttpClient _httpClient;
        private string _urlUserActions = "";
        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        };

        public UserRepository(IHttpClientFactory httpClientFactory, TokenCredentials tokenCredentials)
        {
            _httpClientFactory = httpClientFactory;
            _tokenCredentials = tokenCredentials;
            _httpClient = _httpClientFactory.CreateClient("KeycloakClient");
        }        

        public async Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant)
        {
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
            ]);

            var url = _httpClient.BaseAddress
                .AppendPathSegment("realms")
                .AppendPathSegment(tenant)
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            var response = await _httpClient.PostAsync(url, requestData);

            if (response.IsSuccessStatusCode)
            {                
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenDetails>(content)!;

                SetAuthorizationHeader(result.Access_Token);

                return Result<TokenDetails>.Success(result!);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);
        }

        public async Task<Result<TokenDetails>> LoginAsync(string tenant, User user)
        {
            var urlGetToken = _httpClient.BaseAddress.AppendPathSegment("realms")
                .AppendPathSegment(tenant)
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
                new KeyValuePair<string, string>("username", user.Username!),
                new KeyValuePair<string, string>("password", user.Password!)
            ]);

            var response = await _httpClient.PostAsync(urlGetToken, requestData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenDetails>(content);
                return Result<TokenDetails>.Success(result!);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        public async Task<Result<TokenDetails>> RefreshTokenAsync(string tenant, string refreshToken)
        {
            var urlGetToken = _httpClient.BaseAddress.AppendPathSegment("realms")
                .AppendPathSegment(tenant)
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
            ]);

            var response = await _httpClient.PostAsync(urlGetToken, requestData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TokenDetails>(content);
                return Result<TokenDetails>.Success(result!);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        public async Task<(bool result, string content)> CreateNewUserAsync(string tenant, User user)
        {
            SetBaseUrlUserAction(tenant);

            var json = JsonConvert.SerializeObject(user, Settings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_urlUserActions, httpContent);
            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }

        public async Task<Result<User>> GetUserAsync(string userName)
        {
            var url = _urlUserActions.SetQueryParam("username", userName);
            var response = await _httpClient.GetAsync(url);
            var keycloakUserContent = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<List<User>>(keycloakUserContent)!;
            return Result<User>.Success(user[0]);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string password)
        {
            var url = _urlUserActions
                .AppendPathSegment(userId)
                .AppendPathSegment("reset-password");

            var passwordData = new
            {
                type = "password",
                temporary = false,
                value = password
            };

            var json = JsonConvert.SerializeObject(passwordData, Settings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        public async Task<Result> SendEmailVerificationAsync(string userId)
        {
            var url = _urlUserActions
                .AppendPathSegment(userId);

            var requestData = new
            {
                requiredActions = new string[] { "VERIFY_EMAIL" }
            };

            var json = JsonConvert.SerializeObject(requestData, Settings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                url = url.AppendPathSegment("send-verify-email");
                await _httpClient.PutAsync(url, default!);
                return Result.Success();
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }       

        private void SetBaseUrlUserAction(string tenant)
        {
            _urlUserActions = _httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(tenant)
                    .AppendPathSegment("users");            
        }

        private void SetAuthorizationHeader(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
