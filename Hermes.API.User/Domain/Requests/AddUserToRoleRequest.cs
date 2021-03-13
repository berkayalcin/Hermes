namespace Hermes.API.User.Domain.Requests
{
    public class AddUserToRoleRequest
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}