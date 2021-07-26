using Microsoft.AspNetCore.Components;

namespace DockerGen.Components
{
    public partial class Preview : ComponentBase
    {
        [Parameter]
        public InstructionList Instructions { get; set; }

        public string GetCompiledDockerfile()
        {
            if (Instructions == null)
            {
                return "No instructions found - try adding one now!";
            }

            return Instructions.Compile();
        }
    }
}
