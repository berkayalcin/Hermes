using System;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Services.Advertisement
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;

        public AdvertisementService(IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
        }

        public async Task<AdvertisementDto> Create(CreateAdvertisementRequest createAdvertisementRequest)
        {
            var advertisement = _mapper.Map<Entities.Advertisement>(createAdvertisementRequest);
            advertisement.IsDeleted = false;
            advertisement.CreatedAt = DateTime.UtcNow;
            advertisement = await _advertisementRepository.Create(advertisement);

            // TODO Get Category From Category API
            // TODO Get User From User API
            return _mapper.Map<AdvertisementDto>(advertisement);
        }

        public async Task<AdvertisementDto> Get(Guid id)
        {
            var advertisement = await _advertisementRepository.Get(id);
            return advertisement == null ? null : _mapper.Map<AdvertisementDto>(advertisement);
        }
    }
}