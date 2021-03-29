using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Proxies
{
    public interface IUserApiProxy
    {
        Task<TokenResponse> GetToken();
        Task<UserDto> GetUser(long id);
    }
}