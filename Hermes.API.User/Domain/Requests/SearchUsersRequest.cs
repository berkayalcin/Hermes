namespace Hermes.API.User.Domain.Requests
{
    public class SearchUsersRequest : PagedRequest
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Role { get; set; }
        public long? UserId { get; set; }
    }
}