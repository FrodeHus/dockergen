using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class RunInstruction : Instruction
    {
        public RunInstruction(string shellCommand)
        {
            ShellCommand = shellCommand;
        }
        protected override string Prefix => "RUN";
        public string ShellCommand { get; set; }
        public override string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix.ToUpper());
            builder.Append(ShellCommand);
            return builder.ToString();
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