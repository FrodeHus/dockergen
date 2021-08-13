using DockerGen.Container.Recipes;
using DockerGen.Features.Container.Store;
using Fluxor;
using System.Reflection;

namespace DockerGen.Container
{
    public class ContainerService
    {
        public static List<Recipe> Recipes;

        private List<IDockerInstruction> _validInstructions;
        private readonly RecipeLoader _recipeLoader;
        private readonly IDispatcher _dispatcher;

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
        public ContainerService(RecipeLoader recipeLoader, IDispatcher dispatcher)
        {
            _recipeLoader = recipeLoader;
            _dispatcher = dispatcher;
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
            _dispatcher.Dispatch(new ContainerRecipesLoadedAction(ContainerService.Recipes));
        }
    }
}
