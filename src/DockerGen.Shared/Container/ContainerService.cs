using DockerGen.Container.Recipes;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DockerGen.Container
{
    public class ContainerService
    {
        public static IEnumerable<Recipe> Recipes;

        private List<IDockerInstruction> _validInstructions;
        private readonly RecipeLoader _recipeLoader;

        public static List<string> GetValidPrefixes()
        {
            return new List<string>
            {
                "ADD",
                "ARG",
                "CMD",
                "COPY",
                "ENTRYPOINT",
                "ENV",
                "EXPOSE",
                "EXPOSE",
                "FROM",
                "HEALTHCHECK",
                "LABEL",
                "ONBUILD",
                "PORT",
                "RUN",
                "SHELL",
                "STOPSIGNAL",
                "USER",
                "VOLUME",
                "WORKDIR",
            };
        }
        private readonly ContainerServiceOptions _options;
        public ContainerService(RecipeLoader recipeLoader, IOptions<ContainerServiceOptions> options)
        {
            _recipeLoader = recipeLoader;
            _options = options.Value;
        }

        public Type GetMappedUIComponent(IInstruction instruction)
        {
            return _options.UIComponentMappings[instruction.GetType()];
        }
        public async Task<IEnumerable<Recipe>> GetValidRecipesAsync()
        {
            await EnsureRecipesLoaded();
            return ContainerService.Recipes;
        }

        public List<IDockerInstruction> GetValidInstructions()
        {
            EnsureInstructionInformationLoaded();
            return _validInstructions;
        }

        private void EnsureInstructionInformationLoaded()
        {
            if (_validInstructions != null)
            {
                return;
            }
            var instructions = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IDockerInstruction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && type.GetCustomAttribute<IgnoreInstructionAttribute>() == null ).Select(t => (IDockerInstruction)Activator.CreateInstance(t));
            _validInstructions = instructions.Select(i => i).ToList();
        }

        public async Task EnsureRecipesLoaded()
        {
            if (ContainerService.Recipes != null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_options.RecipePath))
            {
                ContainerService.Recipes = await _recipeLoader.LoadRecipesFromDirectoryAsync(_options.RecipePath); 
            }
            else
            {
                throw new InvalidOperationException("Recipe path has not been set");
            }
        }
    }
}
