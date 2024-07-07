using TokenManager.Infra.CrossCutting.Config;
using TokenManager.Infra.CrossCutting.Extensions;

var builder = WebApplication.CreateBuilder(args);

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);
builder.Services.AddSingleton<ISettings>(applicationSettings);

builder.Services.AddControllers();

builder.Services
    .AddApiAuthentication(applicationSettings.AuthSettings)
    .AddLoggingDependency()
    .AddMediator()
    .AddRepositories(applicationSettings.AuthSettings)
    .AddEndpointsApiExplorer()
    .AddSwagger(applicationSettings!.AuthSettings!);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenManager.Api");
    c.OAuthClientId(applicationSettings!.AuthSettings!.Resource);
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
