using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Proxies;
using Hermes.API.Advertisement.Domain.Repositories.Advertisement;
using Hermes.API.Advertisement.Domain.Repositories.Favorite;

namespace Hermes.API.Advertisement.Domain.Services.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUserApiProxy _userApiProxy;

        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper,
            IAdvertisementRepository advertisementRepository, IUserApiProxy userApiProxy)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _advertisementRepository = advertisementRepository;
            _userApiProxy = userApiProxy;
        }

        public async Task<List<FavoriteDto>> GetAllByUserId(long userId)
        {
            var favorites = await _favoriteRepository.GetAllByUserId(userId);
            return favorites == null ? null : _mapper.Map<List<FavoriteDto>>(favorites);
        }

        public async Task<FavoriteDto> GetByUserIdAndAdvertisementId(long userId, Guid advertisementId)
        {
            var favorite = await _favoriteRepository.GetByUserIdAndAdvertisementId(userId, advertisementId);
            return favorite == null ? null : _mapper.Map<FavoriteDto>(favorite);
        }

        public async Task<FavoriteDto> AddToFavorites(FavoriteDto favoriteDto)
        {
            await _favoriteRepository.DeleteByUserIdAndAdvertisementId(favoriteDto.UserId, favoriteDto.AdvertisementId);
            favoriteDto.User = await _userApiProxy.GetUser(favoriteDto.UserId);
            var favorite = _mapper.Map<Entities.Favorite>(favoriteDto);
            favorite.Advertisement = await _advertisementRepository.Get(favorite.AdvertisementId);
            return _mapper.Map<FavoriteDto>(await _favoriteRepository.Insert(favorite));
        }

        public async Task RemoveFromFavorites(FavoriteDto favoriteDto)
        {
            await _favoriteRepository.DeleteByUserIdAndAdvertisementId(favoriteDto.UserId, favoriteDto.AdvertisementId);
        }
    }
}