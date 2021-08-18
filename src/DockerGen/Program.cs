using Blazored.LocalStorage;
using DockerGen.Container;
using DockerGen.Container.Recipes;
using DockerGen.Infrastructure;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DockerGen
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddHttpClient("DockerGen.Api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DockerGen.Api"));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<RecipeLoader>();
            builder.Services.AddScoped<ContainerService>();
            builder.Services.AddScoped<ClipboardService>();
            builder.Services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.WriteIndented = true;
            });
            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
            });

            builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly).UseReduxDevTools(o =>
            {
                o.UseSystemTextJson((_) =>
                {
                    return new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true,                        
                    };
                });
            }));
            await builder.Build().RunAsync();
        }
    }
}
