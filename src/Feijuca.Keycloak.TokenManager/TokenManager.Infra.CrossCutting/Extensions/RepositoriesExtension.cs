using Feijuca.Keycloak.MultiTenancy.Services;
using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.Extensions.DependencyInjection;

using System;

using TokenManager.Domain.Interfaces;
using TokenManager.Infra.Data.Models;
using TokenManager.Infra.Data.Repositories;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, AuthSettings authSettings)
        {
            services.AddHttpClient("KeycloakClient", client =>
            {
                client.BaseAddress = new Uri(authSettings.AuthServerUrl!);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            var serviceProvider = services.BuildServiceProvider();
            var authService = serviceProvider.GetRequiredService<IAuthService>();

            var tokenCredentials = new TokenCredentials()
            {
                Client_Secret = authService.GetClientSecret(),
                Client_Id = authService.GetClientId(),
                ServerUrl = authService.GetServerUrl()
            };

            services.AddSingleton(tokenCredentials);
            return services;
        }
    }
}
