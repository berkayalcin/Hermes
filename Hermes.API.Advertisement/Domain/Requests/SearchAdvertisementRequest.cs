using System;

namespace Hermes.API.Advertisement.Domain.Requests
{
    public class SearchAdvertisementRequest : PagedRequest
    {
        public Guid? CategoryId { get; set; }
        public long? UserId { get; set; }
        public int? EstimatedBarrowDaysMin { get; set; }
        public int? EstimatedBarrowDaysMax { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}