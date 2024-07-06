using TokenManager.Infra.CrossCutting.Config;
using TokenManager.Infra.CrossCutting.Extensions;

var builder = WebApplication.CreateBuilder(args);

var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("conf/appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"conf/appsettings.{enviroment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);
builder.Services.AddSingleton<ISettings>(applicationSettings);

builder.Services.AddControllers();
builder.Services
    .AddMongo(applicationSettings.MongoSettings)
    .AddLog()
    .AddMediator()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
