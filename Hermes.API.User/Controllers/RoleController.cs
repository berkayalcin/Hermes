using System.Threading.Tasks;
using Hermes.API.User.Domain.Authentication;
using Hermes.API.User.Domain.Constants;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.User.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [HermesAuthorize(UserRoles.Administrator)]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest createRoleRequest)
        {
            var createRoleResult = await _roleService.CreateRoleAsync(createRoleRequest);
            return Created("v1/Role", createRoleResult);
        }
    }
}