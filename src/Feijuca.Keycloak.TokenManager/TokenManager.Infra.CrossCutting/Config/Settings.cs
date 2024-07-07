using Feijuca.Keycloak.MultiTenancy.Services.Models;

namespace TokenManager.Infra.CrossCutting.Config
{
    public interface ISettings
    {
        public AuthSettings AuthSettings { get; }
    }

    public class Settings : ISettings
    {
        public AuthSettings AuthSettings { get; set; } = null!;
    }
}
