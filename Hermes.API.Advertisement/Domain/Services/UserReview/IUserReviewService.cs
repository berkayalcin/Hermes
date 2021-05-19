using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Models;

namespace Hermes.API.Advertisement.Domain.Services.UserReview
{
    public interface IUserReviewService
    {
        Task<UserReviewDto> Create(UserReviewDto userReviewDto);
        Task<List<UserReviewDto>> GetAllByUserId(long userId);
        Task<bool> CheckCanReview(long ownerId, long applicationId);
        Task<List<UserReviewDto>> GetAllByReviewedUserId(long reviewedUserId);
    }
}