using System.Threading.Tasks;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.User.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
        {
            var userDto = await _userService.RegisterAsync(userRegisterRequest);
            return Created("v1/User/", userDto);
        }
    }
}