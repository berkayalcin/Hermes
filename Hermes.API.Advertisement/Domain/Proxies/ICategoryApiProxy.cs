using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Proxies.Models;

namespace Hermes.API.Advertisement.Domain.Proxies
{
    public interface ICategoryApiProxy
    {
        Task<CategoryDto> Get(long id);
    }
}