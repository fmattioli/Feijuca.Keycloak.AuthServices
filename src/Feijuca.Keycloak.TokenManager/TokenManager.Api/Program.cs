using Newtonsoft.Json;
using TokenManager.Infra.CrossCutting.Config;
using TokenManager.Infra.CrossCutting.Extensions;
using TokenManager.Infra.CrossCutting.Handlers;

var builder = WebApplication.CreateBuilder(args);
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Logging
    .ClearProviders()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft", LogLevel.Critical);

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);
Console.WriteLine(JsonConvert.SerializeObject(applicationSettings));

builder.Services.AddSingleton<ISettings>(applicationSettings);

builder.Services.AddControllers();

builder.Services
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddApiAuthentication(applicationSettings.AuthSettings)
    .AddLoggingDependency()
    .AddMediator()
    .AddRepositories(applicationSettings.AuthSettings)
    .AddEndpointsApiExplorer()
    .AddSwagger(applicationSettings!.AuthSettings!);

var app = builder.Build();

app.UseExceptionHandler()
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenManager.Api");
        c.OAuthClientId(applicationSettings!.AuthSettings!.Resource);
        c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    });

app.UseHttpsRedirection()
   .UseAuthorization();

app.MapControllers();

await app.RunAsync();
