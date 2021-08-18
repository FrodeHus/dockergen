using DockerGen.Container;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Infrastructure
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //public async Task<string> CreateQuickShareLinkAsync(ContainerImage containerImage)
        //{
        //    try
        //    {
        //        var result = await _httpClient.PostAsync()
        //    }
        //}
    }
}
