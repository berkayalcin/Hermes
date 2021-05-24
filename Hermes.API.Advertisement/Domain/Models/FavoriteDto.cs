using System;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public Guid AdvertisementId { get; set; }
        public UserDto User { get; set; }
        public AdvertisementDto Advertisement { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}