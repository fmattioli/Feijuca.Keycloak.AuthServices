namespace Feijuca.Keycloak.MultiTenancy.Services.Models
{
    public class AuthSettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? Resource { get; set; }
        public string? AuthServerUrl { get; set; }
        public string? PolicyName { get; set; }
        public IEnumerable<string> Roles { get; set; } = [];
        public IEnumerable<string> Scopes { get; set; } = [];
        public IEnumerable<Realm> Realms { get; set; } = [];
    }

    public class Realm
    {
        public string? Name { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
    }
}
