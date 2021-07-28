using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class FromInstruction : Instruction
    {
        private string _image;
        private string _tag;
        private string _stageName;

        public FromInstruction()
        {

        }
        public FromInstruction(string image, string tag = "latest", string stageName = null)
        {
            Image = image.ToLowerInvariant();
            Tag = tag.ToLowerInvariant();
            StageName = stageName;
        }

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                FireInstructionChanged();
            }
        }
        public string Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                FireInstructionChanged();
            }
        }
        public string StageName
        {
            get { return _stageName; }
            set
            {
                _stageName = value;
                FireInstructionChanged();
            }
        }

        public override string Description => "The FROM instruction initializes a new build stage and sets the Base Image for subsequent instructions. As such, a valid Dockerfile must start with a FROM instruction. ";

        public override string Prefix => "FROM";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(Image);
            if (!string.IsNullOrEmpty(Tag))
            {
                builder.AppendFormat(":{0}", Tag);
            }
            if (!string.IsNullOrEmpty(StageName))
            {
                builder.AppendFormat(" AS {0}", StageName);
            }
        }

        public static implicit operator FromInstruction(string from)
        {
            var match = Regex.Match(from, @"(from\s){0,1}(?<image>[\w\.\/\-]+)(:(?<tag>[\w\.\-]+)){0,1}( as ){0,1}(?<stage>[\w]+){0,1}", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return null;
            }
            var instruction = new FromInstruction(match.Groups["image"].Value, match.Groups["tag"].Value);
            if (match.Groups.ContainsKey("stage"))
            {
                instruction.StageName = match.Groups["stage"].Value;
            }

            return instruction;
        }
    }
}