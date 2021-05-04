using System;
using System.Collections.Generic;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Requests
{
    public class UpdateAdvertisementRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DamageInformation { get; set; }
        public int EstimatedBorrowDays { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public long UserId { get; set; }
        public long CategoryId { get; set; }

        public List<AdvertisementImageDto> Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}