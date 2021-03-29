using System;

namespace Hermes.API.Advertisement.Domain.Proxies.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}