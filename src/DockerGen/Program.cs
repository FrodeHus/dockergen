using Blazored.LocalStorage;
using DockerGen.Components;
using DockerGen.Components.Instructions;
using DockerGen.Container;
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
            builder.Services.Configure<Config>(c => builder.Configuration.Bind("DockerGen", c));
            builder.Services.AddHttpClient("DockerGen.Api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DockerGen.Api"));
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddContainerService(o =>
            {
                o.MapUIComponent<FromInstruction, FromImage>();
                o.MapUIComponent<RunInstruction, RunCommand>();
                o.MapUIComponent<CommandInstruction, Command>();
                o.MapUIComponent<WorkDirInstruction, WorkDir>();
                o.MapUIComponent<AddFilesInstruction, Add>();
                o.MapUIComponent<CopyInstruction, CopyFiles>();
                o.MapUIComponent<ExposeInstruction, Expose>();
                o.MapUIComponent<EntryPointInstruction, EntryPoint>();
                o.MapUIComponent<ArgumentInstruction, Argument>();
                o.MapUIComponent<UserInstruction, User>();
                o.MapUIComponent<EnvironmentInstruction, EnvironmentVariable>();
                o.MapUIComponent<HealthCheckInstruction, HealthCheck>();
                o.MapUIComponent<DynamicRecipe, InstructionRecipe>();
            });
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
