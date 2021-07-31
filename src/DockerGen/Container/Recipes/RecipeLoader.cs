using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DockerGen.Container.Recipes
{
    public class RecipeLoader
    {
        private readonly HttpClient _client;

        private readonly string _zipFilePath = "/recipes.zip";
        public RecipeLoader(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Recipe>> LoadRecipesAsync()
        {
            var recipes = new List<Recipe>();

            var result = await _client.GetAsync(_zipFilePath);
            if (!result.IsSuccessStatusCode)
            {
                return null;
            }
            using var zipStream = await result.Content.ReadAsStreamAsync();
            using var zip = new ZipArchive(zipStream, ZipArchiveMode.Read);
            foreach (var entry in zip.Entries)
            {
                using var stream = new StreamReader(entry.Open());
                var fileData = await stream.ReadToEndAsync();
                recipes.Add(Parse(fileData));
            }
            return recipes;
        }

        public static Recipe Parse(string value)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            return deserializer.Deserialize<Recipe>(value);
        }
    }
}
