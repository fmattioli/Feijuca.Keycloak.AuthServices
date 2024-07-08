<a id="readme-top"></a>

[![MIT License][license-shield]][license-url]

## About The Project
This repository aims to provide a configuration option for .NET projects that are using or planning to use Keycloak for authentication and JWT token generation.
The project consists of two distinct parts. The first one is Feijuca.Keycloak.Auth.MultiTenancy and the second one is Feijuca.Keycloak.TokenManager.

### What is **Feijuca.Keycloak.Auth.MultiTenancy**?
a [nuGet](https://www.nuget.org/packages/Feijuca.Keycloak.MultiTenancy) package that enables the implementation of multi-tenancy concepts using Keycloak. In this context, each realm in Keycloak can represent a different tenant, allowing for unique configurations for each one. This ensures that each tenant within your application can have its own settings within Keycloak.
As a result, it is possible to create usage models where certain tenants have access to specific modules while others do not, all within the same instance of Keycloak.
With this nuGet you can also:
Obtaining a tenant from a token.
Extracting an ID from a token.
Getting the URL where Keycloak is running.
If you wanna get another properties about a tenant, just open a PR and we can discuss it :)
Simplification of managing actions in Keycloak:

### What is **Feijuca.Keycloak.TokenManager**?
Managing certain actions in Keycloak can be complicated. For instance, creating a user involves several steps: obtaining a token, creating the user, and setting a password.
With Feijuca.Keycloak.TokenManager, you can create a user in a single request since all necessary actions are already integrated into the project.
This project also provides additional endpoints, such as:
- Resetting a user's password via email.
- Email confirmation.
- Checking a user's status to determine if they are valid (email confirmed).
- You can also define endpoints according to your needs. If you find them useful for a broader audience, feel free to open a PR in this repository.

### Built With
<img src="https://img.shields.io/badge/dotnet8-blue" />

### Prerequisites
This project was made with the purpose to attend only applications that follows the current [.Net Supported versions.](https://dotnet.microsoft.com/en-us/download/dotnet) 

## Getting Started
Well, as explained previously, this project is just a quicky extension to add multi tenancy support following everything that is done in the [keycloak-authorization-services-dotnet](https://nikiforovall.github.io/keycloak-authorization-services-dotnet/).
So as a first step you should make the settings according to what is described in the documentation for this project and first of all understand what was written on the documentation.

### Installation
_Below is an example of how you can configure your application to accomplish the goal to use multi tenancy besed on keycloak realms_

1. Get a free API Key at [https://example.com](https://example.com)
2. Clone the repo
   ```sh
   git clone https://github.com/your_username_/Project-Name.git
   ```
3. Install NPM packages
   ```sh
   npm install
   ```
4. Enter your API in `config.js`
   ```js
   const API_KEY = 'ENTER YOUR API';
   ```

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.


<!-- CONTACT -->
## Contact
[![LinkedIn][linkedin-shield]][linkedin-url]

[issues-shield]: https://img.shields.io/github/issues/othneildrew/Best-README-Template.svg?style=for-the-badge
[issues-url]: https://github.com/othneildrew/Best-README-Template/issues
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=for-the-badge
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/felipemattioli/
[product-screenshot]: images/screenshot.png
