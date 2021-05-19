using System;

namespace Hermes.API.Advertisement.Domain.Requests
{
    public class SearchAdvertisementApplicationRequest : PagedRequest
    {
        public Guid? AdvertisementId { get; set; }
        public int? ApplicationStatusId { get; set; }
        public long? ApplicantId { get; set; }
    }
}