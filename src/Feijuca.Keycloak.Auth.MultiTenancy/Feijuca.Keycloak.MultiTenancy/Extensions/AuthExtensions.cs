using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Feijuca.Keycloak.Services;
using Feijuca.Keycloak.Services.Models;

namespace Feijuca.Keycloak.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddKeyCloakAuth(this IServiceCollection services, AuthSettings authSettings)
        {
            var httpClient = new HttpClient();
            var tokenHandler = new JwtSecurityTokenHandler();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddKeycloakWebApi(
                    options =>
                    {
                        options.Resource = authSettings.Resource!;
                        options.AuthServerUrl = authSettings.AuthServerUrl;
                        options.VerifyTokenAudience = true;
                    },
                    options =>
                    {
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = async context =>
                            {
                                try
                                {
                                    var tokenJwt = context.Request.Headers.Authorization.FirstOrDefault();

                                    if (string.IsNullOrEmpty(tokenJwt))
                                    {
                                        context.HttpContext.Items["AuthError"] = "Invalid JWT token provided! Please check. ";
                                        context.HttpContext.Items["AuthStatusCode"] = 401;
                                        return;
                                    }

                                    var bearerToken = tokenJwt.Replace("Bearer ", "");
                                    var tokenInfos = tokenHandler.ReadJwtToken(tokenJwt.Replace("Bearer ", ""));
                                    var tenantNumber = tokenInfos.Claims.FirstOrDefault(c => c.Type == "tenant")?.Value;
                                    var tenantRealm = authSettings.Realms.FirstOrDefault(realm => realm.Name == tenantNumber);

                                    if (tenantRealm is null)
                                    {
                                        context.HttpContext.Items["AuthError"] = "This token don't belongs to valid tenant. Please check!";
                                        context.HttpContext.Items["AuthStatusCode"] = 401;
                                        context.NoResult();
                                        return;
                                    }

                                    var audience = tokenInfos.Claims.FirstOrDefault(c => c.Type == "aud")?.Value;
                                    if (string.IsNullOrEmpty(audience))
                                    {
                                        context.HttpContext.Items["AuthError"] = "Invalid scope provided! Please, check the scopes provided!";
                                        context.HttpContext.Items["AuthStatusCode"] = 403;
                                        context.NoResult();
                                        return;
                                    }

                                    var jwksUrl = $"{tenantRealm.Issuer}/protocol/openid-connect/certs";

                                    var jwks = await httpClient.GetStringAsync(jwksUrl);
                                    var jsonWebKeySet = new JsonWebKeySet(jwks);

                                    var tokenValidationParameters = new TokenValidationParameters
                                    {
                                        ValidateIssuer = true,
                                        ValidIssuer = tenantRealm.Issuer,
                                        ValidateAudience = true,
                                        ValidAudience = tenantRealm.Audience,
                                        ValidateLifetime = true,
                                        ValidateIssuerSigningKey = true,
                                        IssuerSigningKeys = jsonWebKeySet.Keys
                                    };

                                    var claims = tokenHandler.ValidateToken(bearerToken, tokenValidationParameters, out var validatedToken);
                                    context.Principal = claims;
                                    context.Success();
                                }
                                catch (Exception e)
                                {
                                    context.Response.StatusCode = 500;
                                    context.HttpContext.Items["AuthError"] = "The following error occurs during the authentication process: " + e.Message;
                                    context.Fail("");
                                }
                            },
                            OnAuthenticationFailed = async context =>
                            {
                                var errorDescription = context.Exception.Message;
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsJsonAsync(errorDescription);
                            },
                            OnChallenge = async context =>
                            {
                                if (!context.Response.HasStarted)
                                {
                                    var errorMessage = context.HttpContext.Items["AuthError"] as string ?? "Authentication failed!";
                                    var statusCode = context.HttpContext.Items["AuthStatusCode"] as int? ?? 401;
                                    var responseMessage = new { Message = errorMessage };
                                    context.Response.StatusCode = statusCode;
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsJsonAsync(responseMessage);
                                }
                                context.HandleResponse();
                            }
                        };
                    }
                );

            services
                .AddAuthorization()
                .AddKeycloakAuthorization()
                .AddAuthorizationBuilder()
                .AddPolicy(
                    authSettings.PolicyName!,
                    policy =>
                    {
                        policy.RequireResourceRolesForClient(
                            authSettings!.Resource!,
                            authSettings!.Roles!.ToArray());
                    }
                );

            services.AddSingleton<JwtSecurityTokenHandler>()
                    .AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
