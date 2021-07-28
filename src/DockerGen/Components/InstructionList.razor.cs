using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Components
{
    public partial class InstructionList : ComponentBase
    {
        [Parameter]
        public EventCallback OnInstructionsChanged { get; set; }

        [Parameter]
        public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();

        private Task HandleDrop()
        {
            return Task.CompletedTask;
        }

        public void AddInstruction(Instruction instruction)
        {
            instruction.OnInstructionChanged += InstructionChanged;

            Instructions.Add(instruction);
            StateHasChanged();
        }

        private void InstructionChanged(object sender, InstructionEventArgs e)
        {
            OnInstructionsChanged.InvokeAsync(this);
        }

        public string Compile()
        {
            if (Instructions == null)
            {
                return "No instructions";
            }
            var builder = new StringBuilder();
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
    }
}
