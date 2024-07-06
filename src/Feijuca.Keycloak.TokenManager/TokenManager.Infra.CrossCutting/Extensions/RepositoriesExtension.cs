using Feijuca.Keycloak.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using TokenManager.Domain.Interfaces;
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
            return services;
        }
    }
}
