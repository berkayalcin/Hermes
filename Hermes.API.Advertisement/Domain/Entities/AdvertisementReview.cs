using System;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class AdvertisementReview
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}