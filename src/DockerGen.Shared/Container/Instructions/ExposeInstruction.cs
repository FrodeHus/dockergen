using System;
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
        public override string DisplayName => "Define exposed ports";
        [Required]
        [RegularExpression("[0-9]+", ErrorMessage = "Port only allows integers")]
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

        public static ExposeInstruction ParseFromString(string line)
        {
            if (!line.StartsWith("EXPOSE", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ParseInstructionException(string.Concat("Not a valid prefix: ", line.AsSpan(0, line.IndexOf(' '))));
            }
            var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (values.Length != 2)
            {
                throw new ParseInstructionException("Not a valid EXPOSE instruction");
            }
            var instruction = new ExposeInstruction();
            var portDefinition = values[1].Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(portDefinition[0], out var port))
            {
                throw new ParseInstructionException("Port is not a number: " + portDefinition[0]);
            }
            instruction.Port = port;
            if (portDefinition.Length == 2)
            {
                instruction.Protocol = portDefinition[1].ToLower();
            }
            else
            {
                instruction.Protocol = "tcp";
            }
            return instruction;
        }
    }
}