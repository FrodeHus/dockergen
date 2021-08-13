using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

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
        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Port only allows integers")]
        [JsonInclude]
        public int Port { get; set; }
        [JsonInclude]
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