using DockerGen.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void RemoveInstruction(Instruction instruction)
        {
            var stage = Stages.SingleOrDefault(s => s.Instructions.Any(i => i.Id == instruction.Id));
            if (stage == null)
            {
                return;
            }

            stage.RemoveInstruction(instruction);
        }
        public bool IsMultiStage()
        {
            return Stages.Count > 1;
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
        public static implicit operator ContainerImage(string dockerinstructions)
        {
            var image = new ContainerImage();
            var lines = dockerinstructions.SplitOnInstructions();
            BuildStage stage = null;
            var validPrefixes = ContainerService.GetValidPrefixes();
            foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l) && l.IndexOf(' ') != -1))
            {
                var instructionType = line.Split(' ')[0].ToUpper();
                if (!validPrefixes.Contains(instructionType))
                {
                    continue;
                }

                Instruction instruction = null;
                switch (instructionType)
                {
                    case "FROM":
                        instruction = (FromInstruction)line;
                        break;
                    case "RUN":
                        instruction = (RunInstruction)line;
                        break;
                    case "COPY":
                        instruction = (CopyInstruction)line;
                        break;
                    default:
                        break;
                }
                if (instruction == null)
                {
                    continue;
                }

                if (instruction is FromInstruction fromInstruction)
                {
                    var stageCount = image.Stages.Count();
                    stage = new BuildStage(fromInstruction, "stage" + stageCount);
                    image.AddStage(stage);
                }
                else if (stage != null && instruction != null)
                {
                    stage.AddInstruction(instruction);
                }
            }
            return image;
        }

    }
}