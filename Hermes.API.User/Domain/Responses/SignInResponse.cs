using System.Collections.Generic;
using System.Security.Claims;

namespace Hermes.API.User.Domain.Responses
{
    public class SignInResponse
    {
        public string Message { get; set; }
        public TokenResponse Token { get; set; }
        public IReadOnlyList<Claim> Claims { get; set; }
    }
}