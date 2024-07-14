using Newtonsoft.Json;

namespace TokenManager.Application.Services.Requests.User
{
    public record AddUserRequest(string Username, string Password, string Email, string FirstName, string LastName, Dictionary<string, string[]> Attributes); 
}
