namespace Feijuca.Keycloak.Services
{
    public interface IAuthService
    {
        int GetTenantFromToken();
        Guid GetUserFromToken();
    }
}
