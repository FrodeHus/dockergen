using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class EnvironmentInstruction : Instruction
	{
		public override string Description => "Defines an environmental variable that can be overriden at runtime";

		public override string Prefix => "ENV";
		public override string DisplayName => "Define environment variable";

		[Required]
		[JsonInclude]
		public string Variable { get; set; }
		[Required]
		[JsonInclude]
		public string Value { get; set; }

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(Variable?.ToUpperInvariant());
			builder.Append('=');
			builder.Append(Value);
		}

        public static EnvironmentInstruction ParseFromString(string line)
        {
			if (!line.StartsWith("ENV", StringComparison.InvariantCultureIgnoreCase))
			{
				throw new ParseInstructionException("Not a valid prefix: " + line[..line.IndexOf(' ')]);
			}

			var envValues = line[line.IndexOf(' ')..].Trim();
			var values = envValues.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);
			if(values.Length != 2)
            {
				throw new ParseInstructionException("Not a valid ENV instruction");
            }
            var instruction = new EnvironmentInstruction
            {
                Variable = values[0].ToUpper(),
                Value = values[1]
            };
            return instruction;
		}
    }
}