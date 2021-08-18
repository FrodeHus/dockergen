using DockerGen.Container;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DockerGen.Infrastructure
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Config _config;

        public ApiService(HttpClient httpClient, IOptions<Config> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<string> CreateQuickShareLinkAsync(ContainerImage containerImage)
        {
            try
            {
                Console.WriteLine(_config.ApiEndpoint);
                var content = new StringContent(JsonSerializer.Serialize(containerImage), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync($"{_config.ApiEndpoint}share/quick", content);
                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }
                var shareUrl = @"{_config.ApiEndpoint}{result.Headers.Location.ToString()}";
                return shareUrl;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<ContainerImage> LoadFromQuickLinkAsync(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<ContainerImage>(_config.ApiEndpoint + id);
            return result;
        }
    }
}
