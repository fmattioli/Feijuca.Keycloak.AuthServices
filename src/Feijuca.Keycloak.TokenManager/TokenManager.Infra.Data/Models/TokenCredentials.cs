namespace TokenManager.Infra.Data.Models
{
    public class TokenCredentials
    {
        public string ServerUrl { get; set; } = null!;
        public string Client_Secret { get; set; } = null!;
        public string Client_Id { get; set; } = null!;
    }
}
