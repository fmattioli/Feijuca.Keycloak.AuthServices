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
    public class UserRepository(IHttpClientFactory httpClientFactory, IAuthService authService, TokenCredentials tokenCredentials) : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAuthService _authService = authService;
        private readonly TokenCredentials _tokenCredentials = tokenCredentials;
        private static readonly JsonSerializerSettings Settings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        };

        public async Task<Result> CreateUserAsync(string tenant, User user)
        {
            var tokenBearerResult = await GetAccessTokenAsync(tenant);
            if (tokenBearerResult.IsSuccess)
            {
                var client = _httpClientFactory
                    .CreateClient("KeycloakClient");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenBearerResult.Value.Access_Token);

                var url = client.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(tenant)
                    .AppendPathSegment("users");

                var json = JsonConvert.SerializeObject(user, Settings);
                StringContent httpContent = new(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
                }

                var responseMessage = await response.Content.ReadAsStringAsync();
                UserErrors.SetTechnicalMessage(responseMessage);
                return Result.Failure(UserErrors.TokenGenerationError);
            }

            return Result.Failure(UserErrors.InvalidUserNameOrPasswordError);
        }

        public async Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant)
        {
            var client = _httpClientFactory.CreateClient("KeycloakClient");

            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
            ]);

            var url = client.BaseAddress
                .AppendPathSegment("realms")
                .AppendPathSegment(tenant)
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            var response = await client.PostAsync(url, requestData);

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

        public async Task<Result<TokenDetails>> LoginAsync(User user)
        {
            var client = _httpClientFactory.CreateClient("KeycloakClient");
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

            var url = client.BaseAddress
                .AppendPathSegment("realms")
                .AppendPathSegment(tenant)
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            var response = await client.PostAsync(url, requestData);

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
