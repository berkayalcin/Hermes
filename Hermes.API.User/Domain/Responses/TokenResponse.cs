using System;

namespace Hermes.API.User.Domain.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}