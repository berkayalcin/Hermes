using System;
using System.Collections.Generic;
using Hermes.API.Advertisement.Domain.Enums;
using Hermes.API.Advertisement.Domain.Models;
using Nest;

namespace Hermes.API.Advertisement.Domain.Entities
{
    public class Advertisement : DocumentEntityBase<Advertisement>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DamageInformation { get; set; }
        public int EstimatedBarrowDays { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        [GeoPoint] public Geolocation Location { get; set; }

        public int StatusId { get; set; }
        public string Status => Enum.GetName(typeof(AdvertisementStatuses), StatusId);

        public long UserId { get; set; }
        public User User { get; set; }

        public long CategoryId { get; set; }
        public Category Category { get; set; }

        public List<AdvertisementImage> Images { get; set; }
        public List<AdvertisementReview> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}