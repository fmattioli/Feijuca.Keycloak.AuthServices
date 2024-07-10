namespace TokenManager.Application.Services.Requests.User
{
    public record AttributesRequest(string Tenant, Dictionary<string, string> UserAttributes);
}
