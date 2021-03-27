using System;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Requests;
using Hermes.API.Advertisement.Domain.Responses;

namespace Hermes.API.Advertisement.Domain.Services.Advertisement
{
    public interface IAdvertisementService
    {
        Task<AdvertisementDto> Create(CreateAdvertisementRequest createAdvertisementRequest);
        Task<AdvertisementDto> Get(Guid id);
        Task<PagedResponse<AdvertisementDto>> GetAll(SearchAdvertisementRequest searchAdvertisementRequest);
    }
}