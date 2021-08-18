using DockerGen.Container;
using System.Net.Http.Json;

namespace DockerGen.Infrastructure
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;


        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CreateQuickShareLinkAsync(ContainerImage containerImage)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("https://localhost:asdf", containerImage);
                return "";
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
