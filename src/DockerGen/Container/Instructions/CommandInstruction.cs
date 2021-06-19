using System.Text;

namespace DockerGen.Container
{
    public class CommandInstruction : Instruction
    {
        public CommandInstruction(params string[] commands)
        {
            Commands = commands;
        }

        public string[] Commands { get; set; }
        protected override string Prefix => "CMD";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append("[");
            for (var i = 0; i < Commands.Length; i++)
            {
                var arg = Commands[i];
                builder.Append("\"");
                builder.Append(arg);
                builder.Append("\"");
                if (i < Commands.Length - 1)
                {
                    builder.Append(", ");
                }
            }
            builder.Append("]");
        }
    }
}