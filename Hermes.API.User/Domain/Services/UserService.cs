using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Hermes.API.User.Domain.Constants;
using Hermes.API.User.Domain.Entities;
using Hermes.API.User.Domain.Exceptions;
using Hermes.API.User.Domain.Extensions;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hermes.API.User.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<HermesUser> _passwordHasher;
        private readonly IRoleService _roleService;
        private readonly UserManager<HermesUser> _userManager;

        public UserService(UserManager<HermesUser> userManager,
            IPasswordHasher<HermesUser> passwordHasher,
            IMapper mapper,
            IRoleService roleService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<UserDto> GetAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ServiceArgumentNullException(nameof(email), ExceptionCodes.RequestedModelIsNull);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var userDto = user.IsDeleted ? null : _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<UserDto> GetAsync(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return user.IsDeleted ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<PagedResponse<SearchUsersResponse>> GetAll(SearchUsersRequest request)
        {
            var query = _userManager
                .Users
                .Include(u => u.UserRoles)
                .ThenInclude(r => r.HermesRole)
                .Where(u => !u.IsDeleted);

            if (!string.IsNullOrEmpty(request.Email))
                query = query.Where(x => x.Email.ToLower().Equals(request.Email.ToLower()));

            if (!string.IsNullOrEmpty(request.Firstname))
                query = query.Where(x => x.Firstname.ToLower().Equals(request.Firstname.ToLower()));

            if (!string.IsNullOrEmpty(request.Lastname))
                query = query.Where(x => x.Lastname.ToLower().Equals(request.Lastname.ToLower()));

            if (!string.IsNullOrEmpty(request.Query))
            {
                request.Query = request.Query.ToLowerInvariant();
                query = query
                    .Where(p =>
                        p.Firstname.ToLower().Contains(request.Query) ||
                        p.Lastname.ToLower().Contains(request.Query) ||
                        p.Email.ToLower().Contains(request.Query) ||
                        p.PhoneNumber.ToLower().Contains(request.Query) ||
                        p.UserRoles.Any(x => x.HermesRole.Name.ToLower().Contains(request.Query))
                    );
            }

            if (!string.IsNullOrEmpty(request.Role))
                query = query.Where(x => x.UserRoles.Any(y => y.HermesRole.Name.Equals(request.Role)));

            if (request.UserId != null) query = query.Where(x => x.Id == request.UserId);

            query = ApplyOrder(query, request.OrderBy, request.OrderByDesc);

            var totalCount = await query.CountAsync();
            var users = await query.ToListAsync();
            var mappedUsers = _mapper.Map<List<SearchUsersResponse>>(users);

            return new PagedResponse<SearchUsersResponse>
            {
                Items = mappedUsers,
                TotalCount = totalCount
            };
        }

        public async Task<UserDto> RegisterAsync(UserRegisterRequest userRegisterRequest)
        {
            if (userRegisterRequest == null)
                throw new ServiceArgumentNullException(nameof(userRegisterRequest),
                    ExceptionCodes.RequestedModelIsNull);

            var isUserExists =
                await _userManager.Users.AnyAsync(u => u.PhoneNumber.Equals(userRegisterRequest.PhoneNumber));
            if (isUserExists)
            {
                throw new InvalidOperationException("User Already Exists");
            }

            var user = _mapper.Map<HermesUser>(userRegisterRequest);
            user.IsDeleted = false;
            user.PasswordHash = _passwordHasher.HashPassword(user, userRegisterRequest.Password);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var createUserResult = await _userManager.CreateAsync(user);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join("\n", createUserResult.Errors.Select(x => x.Description).ToList());
                throw new InvalidOperationException(errors);
            }

            await _roleService.AddUserToRoleAsync(new AddUserToRoleRequest
            {
                Email = user.Email,
                Role = UserRoles.Client
            });

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Password = null;
            scope.Complete();
            return userDto;
        }

        public async Task<ServiceResponseModel> UpdateAsync(UserUpdateRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new ServiceResponseModel($"Cannot find user with {request.Email} email", false);

            var isUserSameOrAdmin = await UserIsSameOrAdmin(user);
            if (!isUserSameOrAdmin) return new ServiceResponseModel("Unauthorized access exception", false);

            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.PhoneNumber = request.PhoneNumber;
            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded) return new ServiceResponseModel("User updated successfully", true);

            var errors = string.Join("\n", updateResult.Errors.Select(x => x.Description).ToList());
            throw new InvalidOperationException(errors);
        }

        public async Task<ServiceResponseModel> DeleteAsync(long id)
        {
            if (id == 0) throw new ServiceArgumentNullException(nameof(id), ExceptionCodes.RequestedModelIsNull);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return new ServiceResponseModel("User Is Not Found", false);
            if (user.IsDeleted) return new ServiceResponseModel("User Is Not Found", false);

            user.IsDeleted = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return new ServiceResponseModel("User deleted successfully", true);

            var errors = string.Join("\n", result.Errors.Select(x => x.Description).ToList());
            return new ServiceResponseModel(errors, false);
        }


        private async Task<bool> UserIsSameOrAdmin(HermesUser hermesUser)
        {
            if (_httpContextAccessor.HttpContext == null) return false;

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser.Id == hermesUser.Id) return true;

            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
            return currentUserRoles?.Any(x => x.Equals(UserRoles.Administrator)) ?? false;
        }

        private IQueryable<HermesUser> ApplyOrder(IQueryable<HermesUser> products,
            string orderBy = "Id",
            bool isDesc = true)
        {
            products = isDesc ? products.OrderByDescending(orderBy) : products.OrderBy(orderBy);
            return products;
        }
    }
}