using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.API.Advertisement.Domain.Repositories.Favorite
{
    public interface IFavoriteRepository
    {
        Task<List<Entities.Favorite>> GetAllByUserId(long userId);
        Task<Entities.Favorite> Insert(Entities.Favorite favorite);
        Task<Entities.Favorite> Update(Entities.Favorite favorite);
        Task Delete(Guid id);
        Task<Entities.Favorite> GetByUserIdAndAdvertisementId(long userId, Guid advertisementId);
        Task DeleteByUserIdAndAdvertisementId(long userId, Guid advertisementId);
    }
}