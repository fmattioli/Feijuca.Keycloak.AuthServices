namespace TokenManager.Domain.Entities
{
    public class TokenDetails
    {
        public string Access_Token { get; set; } = null!;
        public int Expires_In { get; set; }
        public int Refresh_Expires_In { get; set; }
        public string Refresh_Token { get; set; } = null!;
        public string Token_Type { get; set; } = null!;
        public string Scopes { get; set; } = null!;
    }
}
