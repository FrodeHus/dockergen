using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text;

namespace DockerGen.Components
{
    public partial class InstructionList : ComponentBase
    {
        [Parameter]
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();

        public string Compile()
        {
            var builder = new StringBuilder();
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
    }
}
