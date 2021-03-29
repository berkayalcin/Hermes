using System;
using System.Collections.Generic;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Models
{
    public class AdvertisementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DamageInformation { get; set; }
        public int EstimatedBarrowDays { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public long UserId { get; set; }
        public UserDto User { get; set; }

        public long CategoryId { get; set; }
        public CategoryDto Category { get; set; }

        public List<AdvertisementImageDto> Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}