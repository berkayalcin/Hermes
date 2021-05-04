namespace Hermes.API.Advertisement.Domain.Requests
{
    public class SearchAdvertisementRequest : PagedRequest
    {
        public long? CategoryId { get; set; }
        public long? UserId { get; set; }
        public int? EstimatedBorrowDaysMin { get; set; }
        public int? EstimatedBorrowDaysMax { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? StatusId { get; set; }
    }
}