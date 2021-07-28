using System;
using System.Linq;
using System.Text;

namespace DockerGen.Container
{
    public class CopyInstruction : Instruction
    {
        private string source;
        private string destination;
        private string stage;
        private string owner;
        private string group;

        public CopyInstruction()
        {

        }
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

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                FireInstructionChanged();
            }
        }
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                FireInstructionChanged();
            }
        }
        public string Owner
        {
            get { return owner; }
            set
            {
                owner = value;
                FireInstructionChanged();
            }
        }
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                FireInstructionChanged();
            }
        }
        public string Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                FireInstructionChanged();
            }
        }
        public bool IsOwnershipDefined()
        {
            return !string.IsNullOrEmpty(Owner) || !string.IsNullOrEmpty(Group);
        }

        public bool IsLocationDefined()
        {
            return !string.IsNullOrEmpty(Stage);
        }
        public override string Prefix => "COPY";
        public override string DisplayName => "Copy files into container image";

        public override string Description => throw new NotImplementedException();

        protected override void CompileArguments(StringBuilder builder)
        {
            if (IsLocationDefined())
            {
                builder.Append("--from=");
                builder.Append(Stage);
                builder.Append(' ');
            }

            if (IsOwnershipDefined())
            {
                builder.Append("--chown=");
                builder.Append(string.Join(":", Owner, Group).Trim(':'));
                builder.Append(" ");
            }

            builder.Append(Source);
            builder.Append(' ');
            builder.Append(Destination);
        }

        public static implicit operator CopyInstruction(string value)
        {
            var values = value.Split(' ');
            if (!values[0].Equals("COPY", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            var fromStageValue = values.SingleOrDefault(v => v.StartsWith("--from="))?.Split('=');
            var fromStage = fromStageValue?.Length == 2 ? fromStageValue[1] : null;
            var ownerValue = values.SingleOrDefault(v => v.StartsWith("--chown="))?.Split('=');
            var owner = ownerValue?.Length == 2 ? ownerValue[1] : null;
            values = values
                .Where(v => !v.Contains("COPY", StringComparison.OrdinalIgnoreCase) && !v.Contains("from", StringComparison.OrdinalIgnoreCase) && !v.Contains("chown", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var instruction = new CopyInstruction
            {
                Stage = fromStage,
                Owner = owner,
                Source = values[0],
                Destination = values[1]
            };
            return instruction;
        }
    }
}