using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TokenManager.Infra.CrossCutting.Config;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static Settings GetApplicationSettings(this IConfiguration configuration, IHostEnvironment env)
        {
            var settings = configuration.GetSection("Settings").Get<Settings>();

            if (!env.IsDevelopment())
            {
                settings!.MongoSettings!.ConnectionString = GetEnvironmentVariableFromRender("ConnectionString_Mongo");
            }

            return settings!;
        }

        private static string GetEnvironmentVariableFromRender(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? "";
        }
    }
}
