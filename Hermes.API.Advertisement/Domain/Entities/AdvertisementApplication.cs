using System;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class AdvertisementApplication
    {
        public long Id { get; set; }
        public int EstimatedBarrowDays { get; set; }
        public long ApplicantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid AdvertisementId { get; set; }
        public int StatusId { get; set; }
        public bool IsDeleted { get; set; }
    }
}