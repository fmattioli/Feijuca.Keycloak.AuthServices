using Contracts.Common;
using Feijuca.Keycloak.MultiTenancy.Services;
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
        private readonly IAuthService _authService;
        private readonly TokenCredentials _tokenCredentials;
        private readonly HttpClient _httpClient;

        private string urlUserActions = "";

        public UserRepository(IHttpClientFactory httpClientFactory, IAuthService authService, TokenCredentials tokenCredentials)
        {
            _httpClientFactory = httpClientFactory;
            _authService = authService;
            _tokenCredentials = tokenCredentials;
            _httpClient = _httpClientFactory.CreateClient("KeycloakClient");
        }

        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        };

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
                var result = JsonConvert.DeserializeObject<TokenDetails>(content);
                return Result<TokenDetails>.Success(result!);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);
        }

        public async Task<Result> CreateNewUserActions(string tenant, User user)
        {
            var tokenBearerResult = await GetAccessTokenAsync(tenant);
            if (tokenBearerResult.IsSuccess)
            {
                ConfigureHttpClient(tenant, tokenBearerResult.Value.Access_Token);

                var response = await CreateNewUserAsync(user);

                if (response.IsSuccessStatusCode)
                {
                    var keycloakUser = await GetUserAsync(user.Username!);
                    await ResetPasswordAsync(keycloakUser.Value.Id!, user.Password!);
                    return Result.Success();
                }

                var responseMessage = await response.Content.ReadAsStringAsync();
                UserErrors.SetTechnicalMessage(responseMessage);
                return Result.Failure(UserErrors.TokenGenerationError);
            }

            return Result.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        private void ConfigureHttpClient(string tenant, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            urlUserActions = _httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(tenant)
                    .AppendPathSegment("users");
        }

        public async Task<HttpResponseMessage> CreateNewUserAsync(User user)
        {
            var json = JsonConvert.SerializeObject(user, Settings);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(urlUserActions, httpContent);
            return response;
        }

        public async Task<Result<User>> GetUserAsync(string userName)
        {
            var url = urlUserActions.SetQueryParam("username", userName);
            var response = await _httpClient.GetAsync(url);
            var keycloakUserContent = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<List<User>>(keycloakUserContent)!;
            return Result<User>.Success(user[0]);
        }

        public async Task<Result> ResetPasswordAsync(string userId, string password)
        {
            var url = urlUserActions
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
            var url = urlUserActions
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

        public async Task<Result<TokenDetails>> LoginAsync(User user)
        {
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
                new KeyValuePair<string, string>("username", user.Username!),
                new KeyValuePair<string, string>("password", user.Password!),
                new KeyValuePair<string, string>("scope", "tokenmanager-write tokenmanager-read"),
            ]);

            var tenant = _authService.GetTenantFromToken();

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
                var result = JsonConvert.DeserializeObject<TokenDetails>(content);
                return Result<TokenDetails>.Success(result!);
            }

            var responseMessage = await response.Content.ReadAsStringAsync();
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }
    }
}
