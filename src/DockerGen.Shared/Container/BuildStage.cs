using System.Text;

namespace DockerGen.Container
{
    public class BuildStage
    {
        private FromInstruction baseImage;
        public IList<ArgumentInstruction> Arguments = new List<ArgumentInstruction>();

        public FromInstruction BaseImage
        {
            get
            {
                return baseImage;
            }
            set
            {
                baseImage = value;
                baseImage.StageName = StageName;
            }
        }
        public BuildStage()
        {
            //BaseImage = new FromInstruction("scratch", "latest");
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
        public List<IInstruction> Instructions { get; set; } = new List<IInstruction>();

        public void AddInstruction(IInstruction instruction, int index = -1)
        {
            if (index == -1 || index > Instructions.Count)
            {
                Instructions.Add(instruction);
            }
            else
            {
                Instructions.Insert(index, instruction);
            }
        }

        public void RemoveInstruction(IInstruction instruction)
        {
            Instructions.Remove(instruction);
        }

        public string Compile()
        {
            var builder = new StringBuilder();
            foreach (var arg in Arguments)
            {
                builder.AppendLine(arg.Compile());
            }
            if (BaseImage != null)
            {
                builder.AppendLine(BaseImage.Compile());
            }
            foreach (var instruction in Instructions)
            {
                builder.AppendLine(instruction.Compile());
            }
            return builder.ToString();
        }
    }
}