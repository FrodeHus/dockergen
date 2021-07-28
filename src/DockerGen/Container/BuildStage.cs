using System;
using System.Collections.Generic;
using System.Text;

namespace DockerGen.Container
{
    public class BuildStage
    {
        private FromInstruction baseImage;
        private string stageName;

        public event EventHandler<BuildStageEventArgs> OnBuildStageChanged;
        public FromInstruction BaseImage
        {
            get
            {
                return baseImage;
            }
            set
            {
                if (baseImage != null)
                    baseImage.OnInstructionChanged -= InstructionChanged;
                baseImage = value;
                baseImage.StageName = StageName;
                baseImage.OnInstructionChanged += InstructionChanged;
            }
        }
        public BuildStage()
        {
            BaseImage = new FromInstruction("scratch", "latest");
        }

        public BuildStage(FromInstruction baseImage, string name)
        {
            StageName = name;
            BaseImage = baseImage;
        }
        public string StageName
        {
            get { return baseImage?.StageName; }
            set
            {
                if (baseImage != null)
                {
                    baseImage.StageName = value;
                }
            }
        }
        public string Name { get; set; }
        public List<Instruction> Instructions { get; set; } = new List<Instruction>();

        public void AddInstruction(Instruction instruction)
        {
            instruction.OnInstructionChanged += InstructionChanged;
            Instructions.Add(instruction);
            OnBuildStageChanged?.Invoke(this, new BuildStageEventArgs(this));
        }

        public void RemoveInstruction(Instruction instruction)
        {
            instruction.OnInstructionChanged -= InstructionChanged;
            Instructions.Remove(instruction);
            OnBuildStageChanged?.Invoke(this, new BuildStageEventArgs(this));
        }

        private void InstructionChanged(object sender, InstructionEventArgs e)
        {
            OnBuildStageChanged?.Invoke(this, new BuildStageEventArgs(this));
        }

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