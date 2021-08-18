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
		public string DefaultValue { get; set; }

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(Variable?.ToUpperInvariant());
			builder.Append('=');
			builder.Append(DefaultValue);
		}
	}
}