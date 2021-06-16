using System.Text;

namespace DockerGen.Container
{
    public class CopyInstruction : Instruction
    {
        public CopyInstruction(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        public string Source { get; set; }
        public string Destination { get; set; }

        protected override string Prefix => "COPY";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(Source);
            builder.Append(" ");
            builder.Append(Destination);
        }
    }
}