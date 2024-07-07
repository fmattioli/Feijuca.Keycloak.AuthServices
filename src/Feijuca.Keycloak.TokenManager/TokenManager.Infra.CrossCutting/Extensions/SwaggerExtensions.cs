using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TokenManager.Infra.CrossCutting.Filters;

namespace TokenManager.Infra.CrossCutting.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services, AuthSettings keyCloakSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenManager.Api", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(keyCloakSettings.AuthServerUrl!),
                            Scopes = keyCloakSettings.Scopes!.Distinct().ToDictionary(key => key, value => value)
                        }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TokenManager.Api.xml"));
            });
        }
    }
}
