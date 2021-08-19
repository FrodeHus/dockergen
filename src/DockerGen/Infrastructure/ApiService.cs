using DockerGen.Container;
using DockerGen.Container.Recipes;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DockerGen.Infrastructure
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
		private readonly NavigationManager _navigationManager;
		private readonly ILogger<ApiService> _log;
		private readonly Config _config;

        public ApiService(HttpClient httpClient, IOptions<Config> config, NavigationManager navigationManager, ILogger<ApiService> log)
        {
            _httpClient = httpClient;
			_navigationManager = navigationManager;
			_log = log;
			_config = config.Value;
        }

        public async Task<string> CreateQuickShareLinkAsync(ContainerImage containerImage)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(containerImage), Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync($"{_config.ApiEndpoint}share/quick", content);
                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }
                var location = result.Headers.Location.OriginalString.Trim('/');
                var shareUrl = $"{_navigationManager.BaseUri}{location}";
                return shareUrl;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to create quick link");
            }
            return null;
        }

        public async Task<ContainerImage> LoadFromQuickLinkAsync(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<ContainerImage>(_config.ApiEndpoint + id);
            return result;
        }

        public async Task<IEnumerable<Recipe>> LoadRecipesAsync()
		{
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<Recipe>>($"{_config.ApiEndpoint}recipe");
            return result;
		}
    }
}
