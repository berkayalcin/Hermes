namespace Hermes.API.User.Domain.Requests
{
    public class UserUpdateRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}