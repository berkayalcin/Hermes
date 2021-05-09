using System;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class UserReviewDto
    {
        public Guid Id { get; set; }
        public long ApplicationId { get; set; }
        public long ReviewOwnerId { get; set; }
        public long ReviewedUserId { get; set; }
        public UserDto ReviewOwner { get; set; }
        public UserDto ReviewedUser { get; set; }
        public AdvertisementDto Advertisement { get; set; }
        public string Review { get; set; }
        public int Stars { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}