using System.Collections.Generic;
using System.Text;

namespace DockerGen.Container
{
    public class BuildStage
    {
        public FromInstruction BaseImage { get; set; }
        public string Name { get; set; }
        public ICollection<Instruction> Instructions { get; set; } = new List<Instruction>();
        public string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendLine(BaseImage.Compile());
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
    }
}