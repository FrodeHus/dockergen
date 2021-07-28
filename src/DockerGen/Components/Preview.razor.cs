using BlazorMonaco;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DockerGen.Components
{
    public partial class Preview : ComponentBase
    {
        private MonacoEditor _editor;
        [CascadingParameter]
        public Pages.Index ContainerEditor { get; set; }

        protected override void OnInitialized()
        {
            ContainerEditor.Container.OnImageChanged += ContainerChanged;
        }

        private async Task UpdateContainer(KeyboardEvent e)
        {
            var dockerfile = await _editor.GetValue();
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
