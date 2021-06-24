using System;
using System.Text;

namespace DockerGen.Container
{
    public class CopyInstruction : Instruction
    {
        public CopyInstruction(string source, string destination)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException($"'{nameof(source)}' cannot be null or empty.", nameof(source));
            }

            if (string.IsNullOrEmpty(destination))
            {
                throw new ArgumentException($"'{nameof(destination)}' cannot be null or empty.", nameof(destination));
            }

            Source = source;
            Destination = destination;
        }

        public string Source { get; set; }
        public string Destination { get; set; }
        public string Owner { get; set; }
        public string Group { get; set; }
        public string Stage { get; set; }
        public bool IsOwnershipDefined()
        {
            return !string.IsNullOrEmpty(Owner) || !string.IsNullOrEmpty(Group);
        }

        public bool IsLocationDefined()
        {
            return !string.IsNullOrEmpty(Stage);
        }
        protected override string Prefix => "COPY";


        protected override void CompileArguments(StringBuilder builder)
        {
            if (IsLocationDefined())
            {
                builder.Append("--from=");
                builder.Append(Stage);
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