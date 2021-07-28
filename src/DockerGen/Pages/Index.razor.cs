using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        private ContainerImage container = new ContainerImage();

        public ContainerImage Container
        {
            get { return container; }
            set
            {
                if (container != null)
                {
                    container.OnImageChanged -= ContainerChanged;
                }
                container = value;
                container.OnImageChanged += ContainerChanged;
                StateHasChanged();
            }
        }
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
