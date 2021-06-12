using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class FromInstruction : Instruction
    {
        public FromInstruction(string image, string tag = "latest", string stageName = null)
        {
            Image = image.ToLowerInvariant();
            Tag = tag.ToLowerInvariant();
            StageName = stageName;
        }

        public string Image { get; set; }
        public string Tag { get; set; }
        public string StageName { get; set; }
        protected override string Prefix => "FROM";

        public override string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix);
            builder.Append(Image);
            if (!string.IsNullOrEmpty(Tag))
            {
                builder.AppendFormat(":{0}", Tag);
            }
            if (!string.IsNullOrEmpty(StageName))
            {
                builder.AppendFormat(" AS {0}", StageName);
            }

            return builder.ToString();
        }

        public static implicit operator FromInstruction(string from)
        {
            var match = Regex.Match(from, @"(from\s){0,1}(?<image>[\w\.\/]+)(:(?<tag>[\w\.]+)){0,1}");
            if (!match.Success)
            {
                return null;
            }
            return new FromInstruction(match.Groups["image"].Value, match.Groups["tag"].Value);
        }
    }
}