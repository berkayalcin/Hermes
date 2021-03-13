using System.Threading.Tasks;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;

namespace Hermes.API.User.Domain.Services
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(CreateRoleRequest createRoleRequest);
        Task<UserRoleDto> AddUserToRoleAsync(AddUserToRoleRequest addUserToRoleRequest);
        Task<RoleDto> GetRole(string roleName);
    }
}