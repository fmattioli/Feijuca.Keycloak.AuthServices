using Microsoft.Extensions.DependencyInjection;
using Serilog.Formatting.Json;
using Serilog;
using Serilog.Exceptions;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class LogExtension
    {
        public static IServiceCollection AddLoggingDependency(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(new JsonFormatter())
                .CreateLogger();

            return services.AddSingleton(Log.Logger);
        }
    }
}
