using System;

namespace Hermes.API.Advertisement.Domain.Requests
{
    public class SearchAdvertisementApplicationRequest : PagedRequest
    {
        public Guid AdvertisementId { get; set; }
    }
}