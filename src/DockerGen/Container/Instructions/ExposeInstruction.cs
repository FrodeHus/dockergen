using System.Text;

namespace DockerGen.Container
{
	public class ExposeInstruction : Instruction
	{
		private int _port;
		private string _protocol = "tcp";

		public ExposeInstruction()
		{

		}
		public ExposeInstruction(int port, string proto = "tcp")
		{
			Port = port;
			Protocol = proto;
		}
		public override string Description => throw new System.NotImplementedException();

		public override string Prefix => "EXPOSE";
		public override string DisplayName => "Expose ports to the outside";
		public int Port
		{
			get => _port; set
			{
				_port = value;
				FireInstructionChanged();
			}
		}
		public string Protocol
		{
			get => _protocol; set
			{
				_protocol = value;
				FireInstructionChanged();
			}
		}
		protected override void CompileArguments(StringBuilder builder)
		{
			builder.Append(Port);
			builder.Append('/');
			builder.Append(Protocol);
		}

		public static implicit operator ExposeInstruction(string value)
		{
			ExposeInstruction instruction = value;
			return instruction;
		}
	}
}