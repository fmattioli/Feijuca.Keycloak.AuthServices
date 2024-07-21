using Feijuca.Keycloak.MultiTenancy.Services.Models;

namespace Feijuca.Keycloak.MultiTenancy.Services
{
    public interface IAuthService
    {
        string GetInfoFromToken(string infoName);
        string GetTenantFromToken();
        string GetClientId();
        string GetClientSecret();
        string GetServerUrl();
        Realm GetRealm(string realmName);
        Guid GetUserIdFromToken();
    }
}
