using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class RunInstruction : Instruction
    {
        private string _shellCommand;

        public RunInstruction()
        {

        }
        public RunInstruction(string shellCommand)
        {
            if (string.IsNullOrEmpty(shellCommand))
            {
                throw new ArgumentException($"'{nameof(shellCommand)}' cannot be null or empty.", nameof(shellCommand));
            }

            ShellCommand = shellCommand;
        }
        public override string Prefix => "RUN";
        public override string DisplayName => "Execute command in container";
        public virtual string ShellCommand
        {
            get { return _shellCommand; }
            set
            {
                _shellCommand = Validate(value);
                FireInstructionChanged();
            }
        }

        private string Validate(string value)
        {
            return value.Replace("\n", "/\n");
        }

        public override string Description => "The RUN instruction will execute any commands in a new layer on top of the current image and commit the results.";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(ShellCommand);
        }

        public static implicit operator RunInstruction(string v)
        {
            var match = Regex.Match(v, @"(run\s){0,1}(?<cmd>.+$)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return null;
            }

            return new RunInstruction(match.Groups["cmd"].Value);
        }
    }
}