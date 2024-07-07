using Feijuca.Keycloak.MultiTenancy.Services.Models;

namespace Feijuca.Keycloak.MultiTenancy.Services
{
    public interface IAuthService
    {
        string GetTenantFromToken();
        string GetClientId();
        Realm GetRealm(string realmName);
        Guid GetUserFromToken();
    }
}
