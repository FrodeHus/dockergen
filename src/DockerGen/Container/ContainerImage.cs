using System.Collections.Generic;
using System.Text;

namespace DockerGen.Container
{
    public class ContainerImage
    {
        public ICollection<BuildStage> Stages { get; set; } = new List<BuildStage>();
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public string Compile()
        {
            var builder = new StringBuilder();
            foreach (var stage in Stages)
            {
                builder.AppendLine(stage.Compile());
            }
            return builder.ToString().Trim();
        }
    }
}