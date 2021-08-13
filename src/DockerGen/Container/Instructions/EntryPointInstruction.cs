using System.Text;

namespace DockerGen.Container
{
	public class EntryPointInstruction : Instruction
	{
		public EntryPointInstruction()
		{

		}
		public EntryPointInstruction(string cmd)
		{
			var values = cmd.Split(' ');
			Executable = values[0];
		}

		public string Executable { get; set; }

		public string Arguments { get; set; }

		public override string Description => "Setting this value will cause the container to run as an executable ie. it will execute this command and any commands following will be interpreted as parameters.";

		public override string Prefix => "ENTRYPOINT";
		public override string DisplayName => "Set command to run at start";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append('[');
			builder.Append('\"').Append(Executable).Append('\"');
			if (Arguments != null)
			{
				var args = Arguments.Split(' ');
				if (args.Length > 0)
				{
					builder.Append(", ");
					for (var i = 0; i < args.Length; i++)
					{
						var arg = args[i];
						builder.Append('"');
						builder.Append(arg);
						builder.Append('"');
						if (i < args.Length - 1)
						{
							builder.Append(", ");
						}
					}
				}
			}
			builder.Append(']');
		}
	}
}