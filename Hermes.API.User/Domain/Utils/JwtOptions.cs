namespace Hermes.API.User.Domain.Utils
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int DefaultExpirationDays { get; set; }
    }
}