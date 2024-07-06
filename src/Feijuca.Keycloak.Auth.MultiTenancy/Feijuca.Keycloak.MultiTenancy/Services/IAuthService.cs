namespace Feijuca.Keycloak.Services
{
    public interface IAuthService
    {
        string GetTenantFromToken();
        Guid GetUserFromToken();
    }
}
