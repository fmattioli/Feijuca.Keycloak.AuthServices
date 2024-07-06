namespace TokenManager.Domain.Entities
{
    public class GetTokenDetails
    {
        public string ServerUrl { get; set; } = null!;
        public string Grant_Type { get; set; } = null!;
        public string Client_Secret { get; set; } = null!;
        public string Client_Id { get; set; } = null!;
    }
}
