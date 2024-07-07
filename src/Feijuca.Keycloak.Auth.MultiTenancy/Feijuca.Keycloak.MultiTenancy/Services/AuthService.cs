using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Feijuca.Keycloak.MultiTenancy.Services
{
    public class AuthService(IHttpContextAccessor httpContextAccessor, JwtSecurityTokenHandler jwtSecurityTokenHandler, AuthSettings authSettings) : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly JwtSecurityTokenHandler _tokenHandler = jwtSecurityTokenHandler;
        private readonly AuthSettings _authSettings = authSettings;

        public string GetTenantFromToken()
        {
            string jwtToken = GetToken();
            var tokenInfos = _tokenHandler.ReadJwtToken(jwtToken);
            var tenantClaim = tokenInfos.Claims.FirstOrDefault(c => c.Type == "tenant")?.Value!;
            return tenantClaim;
        }

        public Guid GetUserFromToken()
        {
            string jwtToken = GetToken();
            var tokenInfos = _tokenHandler.ReadJwtToken(jwtToken);
            var userClaim = tokenInfos.Claims.FirstOrDefault(c => c.Type == "sub")?.Value!;
            return Guid.Parse(userClaim);
        }

        public string GetClientId()
        {
            return _authSettings.ClientId!;
        }

        public Realm GetRealm(string realmName)
        {
            return _authSettings.Realms.FirstOrDefault(r => r.Name == realmName)!;
        }

        private string GetToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.FirstOrDefault();
            return authorizationHeader!.Replace("Bearer ", string.Empty);
        }
    }
}
