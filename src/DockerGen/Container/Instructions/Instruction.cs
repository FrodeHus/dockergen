using System.Text;

namespace DockerGen.Container
{
    public abstract class Instruction
    {
        protected abstract string Prefix { get; }
        public string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix.ToUpper());
            CompileArguments(builder);
            return builder.ToString();
        }

        protected abstract void CompileArguments(StringBuilder builder);
    }
}