using System.Collections.Generic;

namespace DockerGen.Container
{
    public class ContainerImage
    {
        public ICollection<BuildStage> Stages { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}