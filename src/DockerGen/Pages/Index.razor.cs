using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        public ContainerImage Container { get; set; } = new ContainerImage();
        public Instruction CurrentInstruction { get; set; }

        protected override void OnInitialized()
        {
            Container.OnImageChanged += ContainerChanged;
        }

        private void ContainerChanged(object sender, ContainerImageEventArgs e)
        {
            StateHasChanged();
        }
    }
}
