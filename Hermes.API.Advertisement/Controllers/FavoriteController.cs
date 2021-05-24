using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Authentication;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Services.Favorite;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Advertisement.Controllers
{
    [ApiController, HermesAuthorize, Route("v1/[controller]")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("add-to-favorites")]
        public async Task<IActionResult> AddToFavorites([FromBody] FavoriteDto favoriteDto)
        {
            var favorite = await _favoriteService.AddToFavorites(favoriteDto);
            return Ok(favorite);
        }

        [HttpPut("remove-from-favorites")]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] FavoriteDto favoriteDto)
        {
            await _favoriteService.RemoveFromFavorites(favoriteDto);
            return Ok();
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<IActionResult> Get(long userId)
        {
            var favorites = await _favoriteService.GetAllByUserId(userId);
            if (favorites == null || favorites.Count == 0)
            {
                return NotFound();
            }

            return Ok(favorites);
        }
    }
}