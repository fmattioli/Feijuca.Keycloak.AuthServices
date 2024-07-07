using Newtonsoft.Json;

namespace TokenManager.Application.Services.Requests.User
{
    public record AddUserRequest(string? Username, string? Email, string? FirstName, string? LastName, AttributesRequest? Attributes)
    {
        [JsonIgnore]
        public string? Tenant { get; set; }
    }    
}
