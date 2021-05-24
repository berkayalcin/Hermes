using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Repositories.Favorite;

namespace Hermes.API.Advertisement.Domain.Services.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public FavoriteService(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }

        public async Task<List<FavoriteDto>> GetAllByUserId(long userId)
        {
            var favorites = await _favoriteRepository.GetAllByUserId(userId);
            return favorites == null ? null : _mapper.Map<List<FavoriteDto>>(favorites);
        }

        public async Task<FavoriteDto> AddToFavorites(FavoriteDto favoriteDto)
        {
            await _favoriteRepository.DeleteByUserIdAndAdvertisementId(favoriteDto.UserId, favoriteDto.AdvertisementId);
            var favorite = _mapper.Map<Entities.Favorite>(favoriteDto);
            return _mapper.Map<FavoriteDto>(await _favoriteRepository.Insert(favorite));
        }

        public async Task RemoveFromFavorites(FavoriteDto favoriteDto)
        {
            await _favoriteRepository.DeleteByUserIdAndAdvertisementId(favoriteDto.UserId, favoriteDto.AdvertisementId);
        }
    }
}