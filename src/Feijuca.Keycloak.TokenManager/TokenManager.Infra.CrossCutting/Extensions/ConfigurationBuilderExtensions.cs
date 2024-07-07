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
            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName) => Environment.GetEnvironmentVariable(variableName) ?? "";
    }
}
