namespace TokenManager.Domain.Entities
{
    public class Attributes(string? Zoneinfo, string? Birthdate, string? PhoneNumber, string? Gender, string? Fullname, string? Tenant, string? Picture)
    {
        public string? Zoneinfo { get; set; } = Zoneinfo;
        public string? Birthdate { get; set; } = Birthdate;
        public string? PhoneNumber { get; set; } = PhoneNumber;
        public string? Gender { get; set; } = Gender;
        public string? Fullname { get; set; } = Fullname;
        public string? Tenant { get; set; } = Tenant;
        public string? Picture { get; set; } = Picture;
    }
}
