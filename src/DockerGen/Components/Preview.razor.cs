using BlazorMonaco;
using DockerGen.Features.Container.Store;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components
{
    public partial class Preview : FluxorComponent
    {
        private MonacoEditor _editor;
        [CascadingParameter]
        public Features.Container.Pages.Index ContainerEditor { get; set; }
        [Inject]
        private ILogger<Preview> _logger { get; set; }
        [Inject]
        private IDispatcher Dispatcher { get; set; }
        [Inject]
        private IState<ContainerState> State { get; set; }

        private async Task UpdateContainer(KeyboardEvent _)
        {
            var dockerfile = await _editor.GetValue();
            try
            {
                Dispatcher.Dispatch(new ContainerLoadDockerfileFromStringAction(dockerfile));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import docker instructions");
            }
        }

        public string GetCompiledDockerfile()
        {
            var dockerFile = State.Value.Container.Compile();
            if (string.IsNullOrEmpty(dockerFile))
            {
                return "# No instructions found - try adding one now or copy/paste an existing Dockerfile!";
            }

            return dockerFile;
        }

        private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor _)
        {
            return new StandaloneEditorConstructionOptions
            {
                AutomaticLayout = true,
                FormatOnType = true,
                FormatOnPaste = true,
                Language = "dockerfile",
                Value = GetCompiledDockerfile(),
                Minimap = new EditorMinimapOptions { Enabled = false }
            };
        }
    }
}
