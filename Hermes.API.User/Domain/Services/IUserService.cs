using System.Threading.Tasks;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;

namespace Hermes.API.User.Domain.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterRequest userRegisterRequest);
        Task<UserDto> GetAsync(string email);
        Task<ServiceResponseModel> DeleteAsync(long id);
        Task<ServiceResponseModel> UpdateAsync(long id, UserUpdateRequest request);
        Task<PagedResponse<SearchUsersResponse>> GetAll(SearchUsersRequest request);
        Task<UserDto> GetAsync(long id);
    }
}