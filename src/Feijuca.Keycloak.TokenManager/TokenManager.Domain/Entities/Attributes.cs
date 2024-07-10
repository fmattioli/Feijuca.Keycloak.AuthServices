namespace TokenManager.Domain.Entities
{
    public class Attributes(string Tenant, Dictionary<string, string> UserAttributes)
    {
        public Dictionary<string, string> UserAttributes = UserAttributes;
        public string? Tenant { get; set; } = Tenant;
    }
}
