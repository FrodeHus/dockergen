using System.Collections.Generic;
using System.IO;
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

        public async Task<List<Recipe>> LoadRecipesFromDirectoryAsync(string directory)
        {
            var recipes = new List<Recipe>();
            var files = Directory.GetFiles(directory, "*.yaml");
            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                recipes.Add(Parse(content));
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
