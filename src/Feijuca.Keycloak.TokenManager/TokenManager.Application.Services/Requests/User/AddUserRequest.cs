namespace TokenManager.Application.Services.Requests.User
{
    public class AddUserRequest
    {
        public string? ClientId { get; set; }
        public bool Enabled { get; set; } = true;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool EmailVerified { get; set; } = false;
        public Attributes? Attributes { get; set; }
    }
    public class Attributes
    {
        public string? ZoneInfo { get; set; }
        public string? Birthdate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Fullname { get; set; }
        public string? Tenant { get; set; }
        public string? Picture { get; set; }
    }
}
