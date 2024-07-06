namespace TokenManager.Application.Services.Requests.User
{
    public class AddUserRequest
    {
        public bool Enabled { get; set; } = true;
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool EmailVerified { get; set; } = false;
        public Dictionary<string, string> Attributes { get; set; } = [];
    }
}
