using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hermes.API.User.Domain.Constants;
using Hermes.API.User.Domain.Entities;
using Hermes.API.User.Domain.Exceptions;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;
using Microsoft.AspNetCore.Identity;

namespace Hermes.API.User.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<HermesRole> _roleManager;
        private readonly UserManager<HermesUser> _userManager;

        public RoleService(UserManager<HermesUser> userManager, RoleManager<HermesRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<UserRoleDto> AddUserToRoleAsync(AddUserToRoleRequest addUserToRoleRequest)
        {
            if (addUserToRoleRequest == null)
                throw new ServiceArgumentNullException(nameof(addUserToRoleRequest),
                    ExceptionCodes.RequestedModelIsNull);

            var role = await _roleManager.FindByNameAsync(addUserToRoleRequest.Role);
            if (role == null)
                throw new InvalidOperationException(string.Format(ExceptionConstants.RoleIsNotFound,
                    addUserToRoleRequest.Role));

            var user = await _userManager.FindByEmailAsync(addUserToRoleRequest.Email);

            if (user == null) throw new InvalidOperationException(ExceptionConstants.UserNotFound);


            var addToRoleResult = await _userManager.AddToRoleAsync(user, addUserToRoleRequest.Role);

            if (addToRoleResult.Succeeded)
                return new UserRoleDto
                {
                    RoleId = role.Id,
                    UserId = user.Id,
                    RoleName = role.Name
                };

            var errors = string.Join("\n", addToRoleResult.Errors.Select(x => x.Description).ToList());
            throw new InvalidOperationException(errors);
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleRequest createRoleRequest)
        {
            if (createRoleRequest == null)
                throw new ServiceArgumentNullException(nameof(createRoleRequest),
                    ExceptionCodes.RequestedModelIsNull);

            var existingRole = await _roleManager.FindByNameAsync(createRoleRequest.Name);
            if (existingRole != null)
                return _mapper.Map<RoleDto>(existingRole);

            var role = _mapper.Map<HermesRole>(createRoleRequest);
            var createRoleResult = await _roleManager.CreateAsync(role);

            if (createRoleResult.Succeeded) return _mapper.Map<RoleDto>(role);

            var errors = string.Join("\n", createRoleResult.Errors.Select(x => x.Description).ToList());
            throw new InvalidOperationException(errors);
        }

        public async Task<RoleDto> GetRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return null;
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name
            };
        }
    }
}