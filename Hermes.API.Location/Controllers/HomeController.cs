using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Location.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Location Works");
        }
    }
}