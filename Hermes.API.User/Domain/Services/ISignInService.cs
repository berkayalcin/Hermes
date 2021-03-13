using System.Threading.Tasks;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;

namespace Hermes.API.User.Domain.Services
{
    public interface ISignInService
    {
        Task<SignInResponse> SignInAsync(SignInRequest signInRequest);
        Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest);
    }
}