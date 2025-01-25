namespace WebMarket.Authorization.JWT
{
    public class JWTOptions
    {
        internal const string CookiesName = "tasty-token";
        public required string SecretKey { get; set; }
        public int ExpiresHours { get; set; } = 12;
    }
}
