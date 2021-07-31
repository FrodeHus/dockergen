using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components.Instructions
{
    public partial class RunCommand : ComponentBase, IInstructionComponent
    {
        [CascadingParameter]
        public BuildStage Stage { get; set; }
        [Parameter]
        public RunInstruction Instruction { get; set; }

    }
}
