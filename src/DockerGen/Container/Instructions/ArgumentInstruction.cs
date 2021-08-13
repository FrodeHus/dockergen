using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace DockerGen.Container
{
    public class ArgumentInstruction : Instruction
    {
        public override string Description => throw new System.NotImplementedException();

        public override string Prefix => "ARG";
        public override string DisplayName => "Define build argument";
        [Required]
        public string Argument { get; set; }
        public string Value { get; set; }
        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append(Argument);
            builder.Append('=');
            builder.AppendLine(Value);
        }

        public static implicit operator ArgumentInstruction(string value)
        {
            var values = value.Split(' ');
            if (values.Length != 2 || !values[0].Equals("ARG", System.StringComparison.OrdinalIgnoreCase)) return null;
            var match = Regex.Match(values[1], @"(?<name>[\w]+)={0,1}(?<value>[\w\.-]+)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return null;
            }

            return new ArgumentInstruction
            {
                Argument = match.Groups["name"].Value,
                Value = match.Groups["value"].Value
            };
        }
    }
}