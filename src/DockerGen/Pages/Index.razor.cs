using DockerGen.Components;
using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        private InstructionList _instructions;
        private List<Instruction> _imageInstructions = new()
        {
            new FromInstruction("busybox")
        };
    }
}
