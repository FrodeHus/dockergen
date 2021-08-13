using System.Text;

namespace DockerGen.Container
{
	public class CommandInstruction : Instruction
	{
		public CommandInstruction()
		{

		}
		public CommandInstruction(string command)
		{
			Command = command;
		}

		public string Command { get; set; }

		public override string Description => throw new System.NotImplementedException();

		public override string Prefix => "CMD";
		public override string DisplayName => "Set default command to run if nothing is already specified";

		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append("[");
			var commands = Command?.Split(' ') ?? null;
			if (commands != null)
			{
				for (var i = 0; i < commands.Length; i++)
				{
					var arg = commands[i];
					builder.Append('"');
					builder.Append(arg);
					builder.Append('"');
					if (i < commands.Length - 1)
					{
						builder.Append(", ");
					}
				}
			}
			builder.Append(']');
		}
	}
}