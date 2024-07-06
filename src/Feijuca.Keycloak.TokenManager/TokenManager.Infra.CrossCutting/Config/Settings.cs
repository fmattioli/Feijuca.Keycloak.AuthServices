using Feijuca.Keycloak.Services.Models;

namespace TokenManager.Infra.CrossCutting.Config
{
    public interface ISettings
    {
        public MongoSettings MongoSettings { get; }
        public AuthSettings AuthSettings { get; }
    }

    public class Settings : ISettings
    {
        public AuthSettings AuthSettings { get; set; } = null!;
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
