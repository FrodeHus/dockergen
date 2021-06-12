using System;
using System.Text;

namespace DockerGen.Container
{
    public class FromInstruction : Instruction
    {
        private readonly string _image;
        private readonly string _tag;
        private readonly string _stageName;

        public FromInstruction(string image, string tag = "latest", string stageName = null)
        {
            _image = image.ToLowerInvariant();
            _tag = tag.ToLowerInvariant();
            _stageName = stageName;
        }
        protected override string Prefix => "FROM";

        public override string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix);
            builder.Append(_image);
            if (!string.IsNullOrEmpty(_tag))
            {
                builder.AppendFormat(":{0}", _tag);
            }
            if (!string.IsNullOrEmpty(_stageName))
            {
                builder.AppendFormat(" AS {0}", _stageName);
            }

            return builder.ToString();
        }
    }
}