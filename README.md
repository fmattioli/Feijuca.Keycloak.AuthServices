# WIP

[![MIT License][license-shield]][license-url]

### Built With
<img src="https://img.shields.io/badge/dotnet8-blue" />

### Prerequisites
This project was made with the purpose to attend only applications that follows the current [.Net Supported versions.](https://dotnet.microsoft.com/en-us/download/dotnet) 

## Why feijuca?
Feijuca is a nickname for a famous Brazilian dish called [Feijoada](https://theculturetrip.com/south-america/brazil/articles/a-brief-introduction-to-feijoada-brazils-national-dish). I wanted to use a name representing my born country on this project and Feijuca was chosen.

## About the Project
This repository aims to provide a configuration option for .NET projects that are using or planning to use Keycloak for authentication and JWT token generation. The project consists of two distinct parts:
1. **Feijuca.Keycloak.Auth.MultiTenancy**
2. **Feijuca.Keycloak.TokenManager**

## Feijuca.Keycloak.Auth.MultiTenancy
A [NuGet](https://www.nuget.org/packages/Feijuca.Keycloak.MultiTenancy) package that enables the implementation of multi-tenancy concepts using Keycloak. Each realm in Keycloak can represent a different tenant, allowing for unique configurations for each one. This ensures that each tenant within your application can have its own settings within Keycloak.

### Features
- Obtaining a tenant from a token.
- Extracting an ID from a token.
- Getting the URL where Keycloak is running.
- Custom properties for tenants (open a PR to discuss additional features).
- Simplification of managing actions in Keycloak.

## Feijuca.Keycloak.TokenManager
Managing certain actions in Keycloak can be complicated. For instance, creating a user involves several steps: obtaining a token, creating the user, and setting a password. With **Feijuca.Keycloak.TokenManager**, you can create a user in a single request since all necessary actions are already integrated into the project.

### Features
- Resetting a user's password via email.
- Email confirmation.
- Checking a user's status to determine if they are valid (email confirmed).
- Custom endpoints (open a PR to discuss additional features).

## Getting Started - Multi tenancy configuration
If you wish use  to accomplish the goal to use multi tenancy concept based on each realm on your keycloak instance, here is the steps to configure it:
1. Fill out the appsettings configs related to your realms (tenants)
   ```sh
   {
      "AuthSettings": {
         "Realms": [
            {
               "Name": "yourTenantName1",
               "Audience": "your-audience",
               "Issuer": "https://url-keycloakt/realms/yourTenantName1"
            },
            {
               "Name": "yourTenantName2",
               "Audience": "your-audience",
               "Issuer": "https://url-keycloakt/realms/yourTenantName2"
            },
            {
               "Name": "yourTenantName3",
               "Audience": "documents-processor-api",
               "Issuer": "https://url-keycloakt/realms/yourTenantName3"
            }
      ],
      "ClientSecret": "your-client-secret",
      "ClientId": "your-client-id",
      "Resource": "your-client-id",
      "AuthServerUrl": "https://url-keycloakt/realms/10000/protocol/openid-connect/token"
      }
   }
   ```
2. Configure dependency injection (Note that AuthSettings is a model defined on **Feijuca.Keycloak.Auth.MultiTenancy**, usually I mapped it to variable. for example:
   ```sh
   var settings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
   
   builder.Services
    .AddApiAuthentication(applicationSettings.AuthSettings!);
   
   public static IServiceCollection AddApiAuthentication(this IServiceCollection services, AuthSettings authSettings)
   {
       services.AddHttpContextAccessor();
       services.AddSingleton<JwtSecurityTokenHandler>();
       services.AddKeyCloakAuth(authSettings!);
   
       return services;
   }
   ```

## Getting Started - Using Token Manager Api
If you wish use  to accomplish the goal to use multi tenancy concept based on each realm on your keycloak instance, here is the steps to configure it:
1. Fill out the appsettings configs related to your realms (tenants)
   ```sh
   {
      "AuthSettings": {
         "Realms": [
            {
               "Name": "yourTenantName1",
               "Audience": "your-audience",
               "Issuer": "https://url-keycloakt/realms/yourTenantName1"
            },
            {
               "Name": "yourTenantName2",
               "Audience": "your-audience",
               "Issuer": "https://url-keycloakt/realms/yourTenantName2"
            },
            {
               "Name": "yourTenantName3",
               "Audience": "documents-processor-api",
               "Issuer": "https://url-keycloakt/realms/yourTenantName3"
            }
      ],
      "ClientSecret": "your-client-secret",
      "ClientId": "your-client-id",
      "Resource": "your-client-id",
      "AuthServerUrl": "https://url-keycloakt/realms/10000/protocol/openid-connect/token"
      }
   }
   ```
2. Configure dependency injection (Note that AuthSettings is a model defined on **Feijuca.Keycloak.Auth.MultiTenancy**, usually I mapped it to variable. for example:
   ```sh
   var settings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
   
   builder.Services
    .AddApiAuthentication(applicationSettings.AuthSettings!);
   
   public static IServiceCollection AddApiAuthentication(this IServiceCollection services, AuthSettings authSettings)
   {
       services.AddHttpContextAccessor();
       services.AddSingleton<JwtSecurityTokenHandler>();
       services.AddKeyCloakAuth(authSettings!);
   
       return services;
   }
   ```
   
## Contributing
This is a project in costant evolution, therefore, if you have some suggestion, enter in contact with me or open a pull request and we can discuss.

## License
Distributed under the MIT License. See `LICENSE.txt` for more information.

## Contact
[![LinkedIn][linkedin-shield]][linkedin-url]

[issues-shield]: https://img.shields.io/github/issues/othneildrew/Best-README-Template.svg?style=for-the-badge
[issues-url]: https://github.com/othneildrew/Best-README-Template/issues
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=for-the-badge
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/felipemattioli/
[product-screenshot]: images/screenshot.png
