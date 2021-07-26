using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components.Instructions
{
    public partial class FromImage : ComponentBase, IInstruction
    {
        [CascadingParameter]
        public BuildStage Stage { get; set; }
        [Parameter]
        public FromInstruction Instruction { get; set; }

    }
}
