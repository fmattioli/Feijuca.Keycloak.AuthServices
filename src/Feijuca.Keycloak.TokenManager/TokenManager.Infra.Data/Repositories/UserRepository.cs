using Contracts.Common;
using Feijuca.Keycloak.MultiTenancy.Services;
using Flurl;
using Newtonsoft.Json;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Errors;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Infra.Data.Repositories
{
    public class UserRepository(IHttpClientFactory httpClientFactory, IAuthService authService) : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAuthService _authService = authService;
        private readonly TokenCredentials tokenCredentials = new()
        {
            Grant_Type = "client_credentials",
            Client_Secret = "qSGxtu0CFOmZ6Yzr5ntPK2iXppmKeerS",
            Client_Id = "smartconsig-api"
        };

        public async Task<Result> CreateUserAsync(CreateUser user)
        {
            var tokenBearerResult = await GetAccessTokenAsync();

            if (tokenBearerResult.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Failure(UserErrors.TokenGenerationError);
        }

        public async Task<Result<TokenDetails>> GetAccessTokenAsync()
        {
            var client = _httpClientFactory.CreateClient("KeycloakClient");
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", tokenCredentials.Grant_Type),
                new KeyValuePair<string, string>("client_id", tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", tokenCredentials.Client_Secret),
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

            return Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);
        }

        public async Task<Result<TokenDetails>> LoginAsync(LoginUser user)
        {
            var client = _httpClientFactory.CreateClient("KeycloakClient");
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", tokenCredentials.Client_Secret),
                new KeyValuePair<string, string>("username", user.Username),
                new KeyValuePair<string, string>("password", user.Password),
                new KeyValuePair<string, string>("scope", user.Scopes),
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

            return Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);
        }
    }
}
