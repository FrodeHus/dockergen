using Blazored.LocalStorage;
using DockerGen.Container;
using DockerGen.Container.Recipes;
using DockerGen.Infrastructure;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DockerGen
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<RecipeLoader>();
            builder.Services.AddScoped<ContainerService>();
            builder.Services.AddScoped<ClipboardService>();
            builder.Services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly).UseReduxDevTools());
            await builder.Build().RunAsync();
        }
    }
}
