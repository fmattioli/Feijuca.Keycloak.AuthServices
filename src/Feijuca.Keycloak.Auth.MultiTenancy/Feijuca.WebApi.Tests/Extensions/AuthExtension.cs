using Feijuca.Keycloak.MultiTenancy.Services.Models;
using System.IdentityModel.Tokens.Jwt;
using Feijuca.Keycloak.MultiTenancy.Extensions;

namespace Feijuca.WebApi.Tests.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, AuthSettings authSettings)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddKeyCloakAuth(authSettings!);

            return services;
        }
    }
}
