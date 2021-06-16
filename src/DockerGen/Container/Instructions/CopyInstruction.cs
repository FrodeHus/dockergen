using System;
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
        public string Owner { get; set; }
        public string Group { get; set; }
        public string Location { get; set; }
        public bool IsOwnershipDefined()
        {
            return !string.IsNullOrEmpty(Owner) || !string.IsNullOrEmpty(Group);
        }

        public bool IsLocationDefined()
        {
            return !string.IsNullOrEmpty(Location);
        }
        protected override string Prefix => "COPY";


        protected override void CompileArguments(StringBuilder builder)
        {
            if (IsLocationDefined())
            {
                builder.Append("--from=");
                builder.Append(Location);
                builder.Append(" ");
            }

            if (IsOwnershipDefined())
            {
                builder.Append("--chown=");
                builder.Append(string.Join(":", Owner, Group).Trim(':'));
                builder.Append(" ");
            }

            builder.Append(Source);
            builder.Append(" ");
            builder.Append(Destination);
        }
    }
}