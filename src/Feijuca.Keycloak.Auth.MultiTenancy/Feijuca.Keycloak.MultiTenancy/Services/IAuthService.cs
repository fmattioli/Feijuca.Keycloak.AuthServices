namespace Feijuca.Keycloak.MultiTenancy.Services
{
    public interface IAuthService
    {
        string GetTenantFromToken();
        Guid GetUserFromToken();
    }
}
