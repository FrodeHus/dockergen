using DockerGen.Components.Instructions;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace DockerGen.Container
{
	public class UserInstruction : Instruction
	{

		public UserInstruction()
		{

		}
		public UserInstruction(string user)
		{
			if (string.IsNullOrEmpty(user))
			{
				throw new System.ArgumentException($"'{nameof(user)}' cannot be null or empty.", nameof(user));
			}
			User = user;
		}
		public override Type UIType => typeof(User);

		[Required]
		[JsonInclude]
		public string User { get; set; }

		public override string Description => throw new System.NotImplementedException();

		public override string Prefix => "USER";
		public override string DisplayName => "Set which user runs commands";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(User);
		}
	}
}