using System.Threading.Tasks;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.User.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class SignInController : Controller
    {
        private readonly ISignInService _signInService;

        public SignInController(ISignInService signInService)
        {
            _signInService = signInService;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest signInRequest)
        {
            var response = await _signInService.SignInAsync(signInRequest);
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            await _signInService.ChangePasswordAsync(changePasswordRequest);
            return Ok();
        }
    }
}