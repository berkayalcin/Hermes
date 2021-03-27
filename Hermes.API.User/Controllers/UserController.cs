using System.Threading.Tasks;
using Hermes.API.User.Domain.Authentication;
using Hermes.API.User.Domain.Constants;
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


        [HttpGet("{id}")]
        [HermesAuthorize]
        public async Task<IActionResult> Get(long id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Get([FromQuery] SearchUsersRequest searchUsersRequest)
        {
            var users = await _userService.GetAll(searchUsersRequest);
            if (users.Items == null || users.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegisterRequest)
        {
            var userDto = await _userService.RegisterAsync(userRegisterRequest);
            return Created("v1/User/", userDto);
        }

        [HttpPut("{id}")]
        [HermesAuthorize]
        public async Task<IActionResult> Update(long id, [FromBody] UserUpdateRequest userUpdateRequest)
        {
            var userUpdateResult = await _userService.UpdateAsync(id, userUpdateRequest);
            if (userUpdateResult.Status)
            {
                return Ok(userUpdateResult);
            }

            return BadRequest(userUpdateRequest);
        }

        [HttpDelete("{id}")]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Delete(long id)
        {
            var userDeleteResult = await _userService.DeleteAsync(id);
            if (userDeleteResult.Status)
            {
                return Ok(userDeleteResult);
            }

            return BadRequest(userDeleteResult);
        }
    }
}