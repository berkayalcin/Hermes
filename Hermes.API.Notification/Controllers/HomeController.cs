using System.Threading.Tasks;
using Hermes.API.Notification.Domain.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Notification.Controllers
{
    [ApiController, HermesAuthorize]
    [Route("v1/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Notification Works");
        }
    }
}