using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGen.Container
{
    public class ContainerImage
    {
        public event EventHandler<ContainerImageEventArgs> OnImageChanged;
        public ICollection<BuildStage> Stages { get; set; } = new List<BuildStage>();
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public void AddStage(BuildStage stage)
        {
            stage.OnBuildStageChanged += StageChanged;
            Stages.Add(stage);
            OnImageChanged?.Invoke(this, new ContainerImageEventArgs(this));
        }

        public void RemoveStage(BuildStage stage)
        {
            stage.OnBuildStageChanged -= StageChanged;
            Stages.Remove(stage);
            OnImageChanged?.Invoke(this, new ContainerImageEventArgs(this));
        }

        private void StageChanged(object sender, BuildStageEventArgs e)
        {
            OnImageChanged?.Invoke(this, new ContainerImageEventArgs(this));
        }

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