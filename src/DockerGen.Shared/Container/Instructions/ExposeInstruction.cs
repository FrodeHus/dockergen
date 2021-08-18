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
		public override string Description => @"The EXPOSE instruction informs Docker that the container listens on the specified network ports at runtime. You can specify whether the port listens on TCP or UDP, and the default is TCP if the protocol is not specified.

The EXPOSE instruction does not actually publish the port.It functions as a type of documentation between the person who builds the image and the person who runs the container, about which ports are intended to be published.To actually publish the port when running the container, use the -p flag on docker run to publish and map one or more ports, or the -P flag to publish all exposed ports and map them to high-order ports.";

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