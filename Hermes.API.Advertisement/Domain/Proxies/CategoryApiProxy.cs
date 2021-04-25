using System.Net.Http;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Proxies.Models;
using Newtonsoft.Json;

namespace Hermes.API.Advertisement.Domain.Proxies
{
    public class CategoryApiProxy : ICategoryApiProxy
    {
        private readonly HttpClient _httpClient;
        private readonly IUserApiProxy _userApiProxy;

        public CategoryApiProxy(HttpClient httpClient, IUserApiProxy userApiProxy)
        {
            _httpClient = httpClient;
            _userApiProxy = userApiProxy;
        }

        public async Task<CategoryDto> Get(long id)
        {
            var endpoint = $"/catalog-gateway/v1/Category/{id}";
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", await GetBearerToken());

            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var categoryDto = JsonConvert.DeserializeObject<CategoryDto>(content);
            return categoryDto;
        }

        private async Task<string> GetBearerToken()
        {
            var tokenResponse = await _userApiProxy.GetToken();
            var token = tokenResponse.Token;
            return $"Bearer {token}";
        }
    }
}