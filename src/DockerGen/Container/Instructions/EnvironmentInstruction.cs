using DockerGen.Components.Instructions;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class EnvironmentInstruction : Instruction
	{
		public override string Description => throw new System.NotImplementedException();

		public override string Prefix => "ENV";
		public override string DisplayName => "Define environment variable";
		public override Type UIType => typeof(EnvironmentVariable);

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