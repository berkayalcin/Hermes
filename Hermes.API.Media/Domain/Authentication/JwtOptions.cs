namespace Hermes.API.Media.Domain.Authentication
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int DefaultExpirationDays { get; set; }
    }
}