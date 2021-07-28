using Microsoft.AspNetCore.Components;

namespace DockerGen.Components
{
    public partial class Preview : ComponentBase
    {
        [CascadingParameter]
        public Pages.Index ContainerEditor { get; set; }

        protected override void OnInitialized()
        {
            ContainerEditor.Container.OnImageChanged += ContainerChanged;
        }

        private void ContainerChanged(object sender, Container.ContainerImageEventArgs e)
        {
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
    }
}
