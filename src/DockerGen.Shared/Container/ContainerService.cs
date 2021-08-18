using DockerGen.Container.Recipes;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace DockerGen.Container
{
    public class ContainerService
    {
        public static List<Recipe> Recipes;

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
        public async Task<List<Recipe>> GetValidRecipesAsync()
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
            var instructions = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IDockerInstruction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).Select(t => (IDockerInstruction)Activator.CreateInstance(t));
            _validInstructions = instructions.Select(i => i).ToList();
        }

        private async Task EnsureRecipesLoaded()
        {
            if (ContainerService.Recipes != null)
            {
                return;
            }

            ContainerService.Recipes = await _recipeLoader.LoadRecipesAsync();
        }
    }
}
