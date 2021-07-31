using System.Text;

namespace DockerGen.Container
{
    public class ExposeInstruction : Instruction
    {
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
        public int Port { get; set; }
        public string Protocol { get; set; } = "tcp";

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