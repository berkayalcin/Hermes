namespace Hermes.API.User.Domain.Responses
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string InviteCode { get; set; }
        public bool IsAcceptedEula { get; set; } = true;
    }
}