using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGen.Components
{
    public partial class InstructionList : ComponentBase
    {
        public event EventHandler<EventArgs> OnInstructionsChanged;
        private List<Instruction> _instructions = new();

        [Parameter]
        public List<Instruction> Instructions
        {
            get => _instructions; set
            {
                _instructions = value;
                OnInstructionsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void AddInstruction(Instruction instruction)
        {
            instruction.OnInstructionChanged += InstructionChanged;

            _instructions.Add(instruction);
            OnInstructionsChanged?.Invoke(this, EventArgs.Empty);
            StateHasChanged();
        }

        private void InstructionChanged(object sender, InstructionEventArgs e){
            StateHasChanged();
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
