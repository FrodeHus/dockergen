using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        private ContainerImage container = new ContainerImage();
        public event EventHandler<ContainerImageEventArgs> OnImageChanged;
        public bool IsDragging { get; set; }
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
                OnImageChanged?.Invoke(this, new ContainerImageEventArgs(container));
                StateHasChanged();
            }
        }
        public IInstruction CurrentInstruction { get; set; }

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
