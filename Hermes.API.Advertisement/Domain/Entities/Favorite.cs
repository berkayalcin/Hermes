using System;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class Favorite : DocumentEntityBase<Favorite>
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public Guid AdvertisementId { get; set; }
        public User User { get; set; }
        public Advertisement Advertisement { get; set; }
    }
}