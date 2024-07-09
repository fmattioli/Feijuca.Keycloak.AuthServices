# WIP

[![MIT License][license-shield]][license-url]

### Built With
<img src="https://img.shields.io/badge/dotnet8-blue" />

### Prerequisites
This project was made with the purpose to attend only applications that follows the current [.Net Supported versions.](https://dotnet.microsoft.com/en-us/download/dotnet) 

## Why Feijuca?
Feijuca is a nickname for a famous Brazilian dish called [Feijoada](https://theculturetrip.com/south-america/brazil/articles/a-brief-introduction-to-feijoada-brazils-national-dish). I wanted to use a name representing my born country on this project and Feijuca was chosen.

## About the project
This repository aims to provide a configuration option for .NET projects that are using or planning to use Keycloak for authentication and JWT token generation. The project consists of two distinct parts:
1. **Feijuca.Keycloak.Auth.MultiTenancy**
2. **Feijuca.Keycloak.TokenManager**

**Attention:** 
- The projects work in isolation way, there is no dependency between them. You do not need use one to use other, note that each project has different purpose, below you can understand better:

## Feijuca.Keycloak.Auth.MultiTenancy
A [NuGet](https://www.nuget.org/packages/Feijuca.Keycloak.MultiTenancy) package that enables the implementation of multi-tenancy concepts using Keycloak. Each realm in Keycloak can represent a different tenant, allowing for unique configurations for each one. This ensures that each tenant within your application can have its own settings within Keycloak.

### Features
- You can use all existings keycloak features following a multi tenancy concept based on your realms, so you can handle different configurations based on each tenant (realm).
- With just one instance from your application you can handle different tenants using the same JWT token generation config
- Obtaining information such as a tenant, user id, url and so on from a token. (If you wanna implement a method do retrieve another thing related to the token, open a PR)

## Feijuca.Keycloak.TokenManager
Managing certain actions in Keycloak can be complicated. For instance, creating a new user using the keycloak api involves several steps: obtaining a token, creating the user, setting a password...
With **Feijuca.Keycloak.TokenManager**, you can create a user in a single request since all necessary actions are already integrated into the project.

### Features
- Resetting a user's password via email.
- Email confirmation.
- Checking a user's status to determine if they are valid (email confirmed).
- Custom endpoints (open a PR to discuss additional features).

## Getting Started - Multi tenancy configuration
To accomplish the goal to use multi tenancy concept based on each realm (Where each realm would be a different tenant), here is the steps to configure it:
I assume that you already had the configurations created on your keycloak instance, as for example, a client_id created with their configurations related created.
Starting from this point, to use **Feijuca.Keycloak.Auth.MultiTenancy** follow the steps below:
1. The tenant that each user belongs is stored on a user attribute, the tenant value should be the name of the realm. You can create a new attribute manually or using **Feijuca.Keycloak.TokenManager** you can create a user with these  default attributes below:
![image](https://github.com/fmattioli/Feijuca.Keycloak.AuthServices/assets/27566574/8dcf2109-2145-4e53-9487-ab8fe2582fff)

2. Create a new audience related to the scopes from your client and include this audience on your client:
This step is mandatory because on each request received the tool will validate the tokena audience following what was filled out on step 3.
![image](https://github.com/fmattioli/Feijuca.Keycloak.AuthServices/assets/27566574/6b7b437e-fa29-4776-b29f-4dba8e6d1f21)

3. Filled out appsettings file on your application, relate all of yours realms (tenants)
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
      "AuthServerUrl": "https://url-keycloak"
      }
   }
   ```
4. Configure dependency injection (Note that AuthSettings is a model defined on **Feijuca.Keycloak.Auth.MultiTenancy**, I recommend you use the GetSection method to map the appsettings configs to the AuthSettings model:
   ```sh
   var settings = configuration.GetSection("AuthSettings").Get<AuthSettings>();
   ```
   
5. Add the service to the service collection from your application, I recommend you create a new extension method as below:
   ```sh   
   builder.Services
    .AddApiAuthentication(applicationSettings.AuthSettings!);
   
   public static class AuthExtension
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, AuthSettings authSettings)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddKeyCloakAuth(authSettings!);

            return services;
        }
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
      "AuthServerUrl": "https://url-keycloakt"
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
