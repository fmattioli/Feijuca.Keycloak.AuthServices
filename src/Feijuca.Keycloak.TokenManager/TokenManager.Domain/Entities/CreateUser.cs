namespace TokenManager.Domain.Entities
{
    public record CreateUser(string? Username, string? Email, string? FirstName, string? LastName, Attributes Attributes)
    {
        public bool Enabled { get; set; } = true;
        public bool EmailVerified { get; set; } = false;
        public Attributes? Attributes { get; set; } = Attributes;
    }

    public record Attributes(string? Zoneinfo, string? Birthdate, string? PhoneNumber, string? Gender, string? Fullname, string? Tenant, string? Picture);
}
