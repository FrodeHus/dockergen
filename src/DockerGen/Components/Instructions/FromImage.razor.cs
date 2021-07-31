using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Components.Instructions
{
    public partial class FromImage : ComponentBase, IInstructionComponent
    {
        [Parameter]
        public FromInstruction Instruction { get; set; }
    }
}
