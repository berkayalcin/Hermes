using System.Threading.Tasks;
using Hermes.API.Media.Domain.Authentication;
using Hermes.API.Media.Domain.Requests;
using Hermes.API.Media.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Media.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        [HermesAuthorize]
        public async Task<IActionResult> Upload([FromBody] UploadImageRequest uploadImageRequest)
        {
            var result = await _imageService.Upload(uploadImageRequest);
            return Ok(result);
        }
    }
}