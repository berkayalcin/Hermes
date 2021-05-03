using System;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class AdvertisementApplicationDto
    {
        public long Id { get; set; }
        public int EstimatedBarrowDays { get; set; }
        public long ApplicantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid AdvertisementId { get; set; }
        public bool IsDeleted { get; set; }
    }
}