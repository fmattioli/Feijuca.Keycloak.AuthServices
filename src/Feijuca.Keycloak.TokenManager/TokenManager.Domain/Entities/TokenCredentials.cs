namespace TokenManager.Domain.Entities
{
    public class TokenCredentials
    {
        public string ServerUrl { get; set; } = null!;
        public string Grant_Type { get; set; } = null!;
        public string Client_Secret { get; set; } = null!;
        public string Client_Id { get; set; } = null!;
    }
}
