using System;
using Hermes.API.Advertisement.Domain.Enums;
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
        public int StatusId { get; set; }
        public string Status => Enum.GetName(typeof(AdvertisementApplicationStatuses), StatusId);
        public bool IsDeleted { get; set; }
    }
}