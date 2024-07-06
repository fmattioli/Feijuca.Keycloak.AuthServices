using Contracts.Common;

using Feijuca.Keycloak.Services;

using Flurl;
using TokenManager.Domain.Entities;
using TokenManager.Domain.Errors;
using TokenManager.Domain.Interfaces;

namespace TokenManager.Infra.Data.Repositories
{
    public class UserRepository(IHttpClientFactory httpClientFactory, IAuthService authService) : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAuthService _authService = authService;

        public async Task<Result> CreateUser(User user)
        {
            var tokenBearerResult = await GetAccessToken(new GetTokenDetails
            {
                Grant_Type = "client_credentials",
                Client_Secret = "qSGxtu0CFOmZ6Yzr5ntPK2iXppmKeerS",
                Client_Id = "smartconsig-api"
            });

            if (tokenBearerResult.IsSuccess)
            {
                return Result.Success();
            }

            return Result.Failure(UserErrors.TokenGenerationError);
        }

        public async Task<Result<string>> GetAccessToken(GetTokenDetails getTokenDetails)
        {            
            var client = _httpClientFactory.CreateClient("KeycloakClient");
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", getTokenDetails.Grant_Type),
                new KeyValuePair<string, string>("client_id", getTokenDetails.Client_Id),
                new KeyValuePair<string, string>("client_secret", getTokenDetails.Client_Secret),
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
                var content = response.Content.ReadAsStringAsync();
                return Result<string>.Success("");
            }

            return Result<string>.Failure(UserErrors.TokenGenerationError);
        }
    }
}
