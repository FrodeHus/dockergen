using DockerGen.Container.Recipes;
using Microsoft.Extensions.DependencyInjection;

namespace DockerGen.Container
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContainerService(this IServiceCollection services, Action<ContainerServiceOptions> configureOptions = null)
        {
            if(configureOptions == null)
            {
                services.AddOptions<ContainerServiceOptions>().Configure(o => { });
            }
            else
            {
                services.Configure(configureOptions);
            }

            services.AddScoped<RecipeLoader>();
            services.AddScoped<ContainerService>();

            return services;
        }
    }

    public class ContainerServiceOptions
    {
        internal readonly Dictionary<Type, Type> UIComponentMappings = new();

        public void MapUIComponent<TInstruction, TUIComponent>() where TInstruction : IInstruction
        {
            UIComponentMappings.Add(typeof(TInstruction), typeof(TUIComponent));
        }

        public string RecipePath { get; set; }
    }
}
