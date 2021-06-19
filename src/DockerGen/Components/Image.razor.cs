using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components
{
    public partial class Image : ComponentBase
    {
        public ContainerImage ContainerImage { get; set; } = new ContainerImage();
    }
}