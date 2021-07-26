using System.Threading.Tasks;
using DockerGen.Components;
using DockerGen.Container;
using Microsoft.AspNetCore.Components;

namespace DockerGen.Pages
{
    public partial class Index : ComponentBase
    {
        private InstructionList _instructions;
        protected void AddInstruction()
        {
            _instructions.AddInstruction(new FromInstruction("busybox"));
        }

        private Task InstructionsUpdated()
        {
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}
