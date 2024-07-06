namespace Feijuca.Keycloak.MultiTenancy.Services.Models
{
    public record AuthSettings(string? ClientId, string? Resource, string? AuthServerUrl, string? PolicyName, IEnumerable<string>? Roles, IEnumerable<string>? Scopes)
    {
        public IEnumerable<Realm> Realms { get; set; } = [];
    }

    public record Realm(string? Name, string? Audience, string? Issuer);
}
