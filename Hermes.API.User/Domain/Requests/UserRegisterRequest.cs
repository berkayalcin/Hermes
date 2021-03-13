namespace Hermes.API.User.Domain.Requests
{
    public sealed class UserRegisterRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsAcceptedEula { get; set; } = true;
    }
}