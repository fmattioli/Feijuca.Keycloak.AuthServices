namespace TokenManager.Application.Services.Requests.User
{
    public record AttributesRequest(string? Tenant, string? ZoneInfo, string? Birthdate, string? PhoneNumber, string? Gender, string? Fullname, string? Picture);
}
