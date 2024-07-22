using Feijuca.Keycloak.MultiTenancy.Services.Models;
using Feijuca.WebApi.Tests.Filters;
using Flurl;
using Microsoft.OpenApi.Models;

namespace Feijuca.WebApi.Tests.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services, AuthSettings keyCloakSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Receipts.CommandHandler.API", Version = "v1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(keyCloakSettings.AuthServerUrl.AppendPathSegment("/realms/10000/protocol/openid-connect/token")),
                            Scopes = keyCloakSettings.Scopes!.ToDictionary(key => key, value => value)
                        }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Receipts.CommandHandler.API.xml"));
            });
        }
    }
}
