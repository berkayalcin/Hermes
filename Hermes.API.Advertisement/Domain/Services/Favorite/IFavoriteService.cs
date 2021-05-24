using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Services.Favorite
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDto>> GetAllByUserId(long userId);
        Task<FavoriteDto> AddToFavorites(FavoriteDto favoriteDto);
        Task RemoveFromFavorites(FavoriteDto favoriteDto);
    }
}