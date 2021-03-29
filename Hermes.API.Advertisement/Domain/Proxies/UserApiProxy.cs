using System.Net.Http;
using System.Threading.Tasks;
using Hermes.API.Advertisement.Domain.Proxies.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Hermes.API.Advertisement.Domain.Proxies
{
    public class UserApiProxy : IUserApiProxy
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UserApiProxy(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<TokenResponse> GetToken()
        {
            var username = _configuration.GetSection("AppUser")["Username"];
            var password = _configuration.GetSection("AppUser")["Password"];
            var endpoint = $"/user-gateway/v1/Signin/";

            var signInRequest = new SignInRequest
            {
                Email = username,
                Password = password
            };
            var signInRequestBody = JsonConvert.SerializeObject(signInRequest);
            var httpContent = new StringContent(signInRequestBody, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var signInResponse = JsonConvert.DeserializeObject<SignInResponse>(content);
            return signInResponse.Token;
        }

        public async Task<UserDto> GetUser(long id)
        {
            var endpoint = $"/user-gateway/v1/User/{id}";
            var token = await GetBearerToken();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(content);
            return userDto;
        }

        private async Task<string> GetBearerToken()
        {
            var tokenResponse = await GetToken();
            return tokenResponse.Token;
        }
    }
}