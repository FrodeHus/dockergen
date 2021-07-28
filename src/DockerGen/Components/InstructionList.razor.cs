using DockerGen.Container;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DockerGen.Components
{
    public partial class InstructionList : ComponentBase
    {
        [CascadingParameter]
        private Pages.Index ContainerEditor { get; set; }
        [Parameter]
        public EventCallback OnInstructionsChanged { get; set; }

        [Parameter]
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();

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

        private List<string> GetBuildStageNames()
        {
            return ContainerEditor.Container.Stages.Select(s => s.StageName).ToList();
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
