using DockerGen.Container.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DockerGen.Container
{
    public class ContainerService
    {
        private List<IDockerInstruction> _validInstructions;
        private List<Recipe> _validRecipes;
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
        public ContainerService(RecipeLoader recipeLoader)
        {
            _recipeLoader = recipeLoader;
        }
        public async Task<List<Recipe>> GetValidRecipesAsync()
        {
            await EnsureRecipesLoaded();
            return _validRecipes;
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
            if (_validRecipes != null)
            {
                return;
            }

            _validRecipes = await _recipeLoader.LoadRecipesAsync();
        }
    }
}
