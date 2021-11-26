using DockerGen.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
    [JsonConverter(typeof(ContainerConverter))]
    public class ContainerImage
    {
        public IList<BuildStage> Stages { get; set; } = new List<BuildStage>();

        public void AddStage(BuildStage stage)
        {
            Stages.Add(stage);
        }

        public void RemoveStage(BuildStage stage)
        {
            Stages.Remove(stage);
        }

        public void RemoveInstruction(IInstruction instruction)
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
        public static ContainerImage ParseFromString(string dockerinstructions)
        {
            var image = new ContainerImage();
            var lines = dockerinstructions.SplitOnInstructions();
            BuildStage stage = null;
            foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l) && l.IndexOf(' ') != -1))
            {
                var instructionType = line.Split(' ')[0].ToUpper();

                IInstruction instruction = null;
                switch (instructionType)
                {
                    case "ARG":
                        instruction = ArgumentInstruction.ParseFromString(line);
                        break;
                    case "FROM":
                        instruction = FromInstruction.ParseFromString(line);
                        break;
                    case "RUN":
                        instruction = RunInstruction.ParseFromString(line);
                        break;
                    case "COPY":
                        instruction = CopyInstruction.ParseFromString(line);
                        break;
                    case "ENV":
                        instruction = EnvironmentInstruction.ParseFromString(line);
                        break;
                    case "EXPOSE":
                        instruction = ExposeInstruction.ParseFromString(line);
                        break;
                    case "WORKDIR":
                        instruction = WorkDirInstruction.ParseFromString(line);
                        break;
                    case "USER":
                        instruction = UserInstruction.ParseFromString(line);
                        break;
                    case "ENTRYPOINT":
                        instruction = EntryPointInstruction.ParseFromString(line);
                        break;
                    case "CMD":
                        instruction = CommandInstruction.ParseFromString(line);
                        break;
                    case "ADD":
                        instruction = AddFilesInstruction.ParseFromString(line);
                        break;
                    case "HEALTHCHECK":
                        instruction = HealthCheckInstruction.ParseFromString(line);
                        break;
                    default:
                        instruction = new NotImplementedInstruction(instructionType);
                        break;
                }
                if (instruction == null)
                {
                    throw new ParseInstructionException("Unknown instruction - " + line);
                }

                if (instruction is ArgumentInstruction argumentInstruction && (image.Stages.Count == 0 || image.Stages[0].BaseImage == null))
                {
                    var stageCount = image.Stages.Count;
                    stage = new BuildStage();
                    stage.Arguments.Add(argumentInstruction);
                    stage.StageName = "stage" + image.Stages.Count;
                    image.AddStage(stage);
                    continue;
                }

                if (instruction is FromInstruction fromInstruction)
                {
                    if (image.Stages.LastOrDefault() is BuildStage buildstage && buildstage.BaseImage == null)
                    {
                        buildstage.BaseImage = fromInstruction;
                    }
                    else
                    {
                        var stageCount = image.Stages.Count;
                        stage = new BuildStage(fromInstruction, "stage" + stageCount);
                        image.AddStage(stage);
                    }
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