using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Components
{
    public partial class InstructionList : ComponentBase
    {
        [Parameter]
        public EventCallback OnInstructionsChanged { get; set; }
        private List<Instruction> _instructions = new();

        [Parameter]
        public List<Instruction> Instructions
        {
            get => _instructions; set
            {
                _instructions = value;
            }
        }

        private Task HandleDrop()
        {
            return Task.CompletedTask;
        }

        public void AddInstruction(Instruction instruction)
        {
            instruction.OnInstructionChanged += InstructionChanged;

            _instructions.Add(instruction);
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
