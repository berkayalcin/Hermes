using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Authentication;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Services.UserReview;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Advertisement.Controllers
{
    [ApiController, Route("v1/[controller]")]
    [HermesAuthorize]
    public class UserReviewController : Controller
    {
        private readonly IUserReviewService _userReviewService;

        public UserReviewController(IUserReviewService userReviewService)
        {
            _userReviewService = userReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserReviewDto userReview)
        {
            var userReviewDto = await _userReviewService.Create(userReview);
            return Created("v1/userReview", userReviewDto);
        }

        [HttpGet("byOwnerId/{ownerId}")]
        public async Task<IActionResult> Get(long ownerId)
        {
            var userReviews = await _userReviewService.GetAllByUserId(ownerId);
            if (userReviews == null)
                return NotFound();
            return Ok(userReviews);
        }

        [HttpGet("can-review/{ownerId}/{applicationId}")]
        public async Task<IActionResult> Get(long ownerId, long applicationId)
        {
            var canReview = await _userReviewService.CheckCanReview(ownerId, applicationId);
            return Ok(canReview);
        }
    }
}