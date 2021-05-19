using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.API.Advertisement.Domain.Repositories.UserReview
{
    public interface IUserReviewRepository
    {
        Task<Entities.UserReview> Create(Entities.UserReview userReview);
        Task<Entities.UserReview> Update(Entities.UserReview userReview);
        Task Delete(Guid id);
        Task<Entities.UserReview> Get(Guid id);
        Task<List<Entities.UserReview>> GetAll();
        Task<List<Entities.UserReview>> GetAllByOwnerId(long ownerId);
        Task<List<Entities.UserReview>> GetAllByOwnerIdAndApplicationId(long ownerId, long applicationId);
        Task<List<Entities.UserReview>> GetAllByReviewedUserId(long reviewedUserId);
    }
}