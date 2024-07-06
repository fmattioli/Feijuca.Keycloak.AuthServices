namespace TokenManager.Domain.Entities
{
    public record User(string? Username, string? Email, string? FirstName, string? LastName)
    {
        public bool Enabled { get; set; } = true;
        public bool EmailVerified { get; set; } = false;
        public Dictionary<string, string> Attributes { get; set; } = [];
    }
}
