using System.Text;

namespace DockerGen.Container
{
    public class EntryPointInstruction : Instruction
    {
        public EntryPointInstruction(string executable, params string[] arguments)
        {
            Executable = executable;
            Arguments = arguments;
        }
        public string Executable { get; set; }
        public string[] Arguments { get; set; }
        protected override string Prefix => "ENTRYPOINT";

        protected override void CompileArguments(StringBuilder builder)
        {
            builder.Append("[");
            builder.Append($"\"{Executable}\"");
            if (Arguments.Length > 0)
            {
                builder.Append(", ");
                for (var i = 0; i < Arguments.Length; i++)
                {
                    var arg = Arguments[i];
                    builder.Append("\"");
                    builder.Append(arg);
                    builder.Append("\"");
                    if (i < Arguments.Length - 1)
                    {
                        builder.Append(", ");
                    }
                }
            }
            builder.Append("]");
        }
    }
}