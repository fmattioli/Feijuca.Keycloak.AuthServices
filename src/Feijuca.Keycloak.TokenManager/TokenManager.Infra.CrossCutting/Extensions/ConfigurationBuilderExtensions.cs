using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TokenManager.Infra.CrossCutting.Config;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Settings GetApplicationSettings(this IConfiguration configuration, IHostEnvironment env)
        {
            var settings = configuration.GetSection("Settings").Get<Settings>()!;

            if (!env.IsDevelopment())
            {
                settings.AuthSettings.Realms =
                [
                    new Realm
                    { 
                        Name = GetEnvironmentVariableFromRender("Realm.Name"),
                        Audience = GetEnvironmentVariableFromRender("Realm.Audience"), 
                        Issuer = GetEnvironmentVariableFromRender("Realm.Issuer")
                    }
                ];

                settings.AuthSettings.AuthServerUrl = GetEnvironmentVariableFromRender("AuthServerUrl");
                settings.AuthSettings.ClientId = GetEnvironmentVariableFromRender("ClientId");
                settings.AuthSettings.ClientSecret = GetEnvironmentVariableFromRender("ClientSecret");
            }
            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName) => Environment.GetEnvironmentVariable(variableName) ?? "";
    }
}
