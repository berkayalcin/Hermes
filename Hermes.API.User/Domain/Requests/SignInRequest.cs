namespace Hermes.API.User.Domain.Requests
{
    public class SignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}