using System.Collections.Generic;

namespace DockerGen.Container
{
    public class BuildStage
    {
        public FromInstruction BaseImage { get; set; }
        public string Name { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
    }
}