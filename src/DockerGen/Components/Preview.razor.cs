using BlazorMonaco;
using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DockerGen.Components
{
    public partial class Preview : ComponentBase
    {
        private MonacoEditor _editor;
        [CascadingParameter]
        public Pages.Index ContainerEditor { get; set; }
        [Inject]
        private ILogger<Preview> _logger { get; set; }
        protected override void OnInitialized()
        {
            ContainerEditor.Container.OnImageChanged += ContainerChanged;
        }

        private async Task UpdateContainer(KeyboardEvent _)
        {
            var dockerfile = await _editor.GetValue();
            try
            {
                ContainerImage image = dockerfile;
                if (image != null)
                {
                    ContainerEditor.Container = dockerfile;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to import docker instructions");
            }
        }

        private void ContainerChanged(object sender, Container.ContainerImageEventArgs e)
        {
            _editor.SetValue(GetCompiledDockerfile());
            StateHasChanged();
        }

        public string GetCompiledDockerfile()
        {
            var dockerFile = ContainerEditor.Container.Compile();
            if (string.IsNullOrEmpty(dockerFile))
            {
                return "No instructions found - try adding one now!";
            }

            return dockerFile;
        }

        private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                AutomaticLayout = true,
                Language = "dockerfile",
                Value = GetCompiledDockerfile(),
                Minimap = new EditorMinimapOptions { Enabled = false }
            };
        }
    }
}
