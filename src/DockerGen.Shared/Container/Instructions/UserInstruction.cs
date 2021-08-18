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

		[Required]
		[JsonInclude]
		public string User { get; set; }

		public override string Description => @"The USER instruction sets the user name (or UID) and optionally the user group (or GID) to use when running the image and for any RUN, CMD and ENTRYPOINT instructions that follow it in the Dockerfile.";

		public override string Prefix => "USER";
		public override string DisplayName => "Set which user runs commands";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(User);
		}
	}
}