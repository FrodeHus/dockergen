using System.Text;

namespace DockerGen.Container
{
    public abstract class Instruction
    {
        public abstract string Description { get; }
        protected abstract string Prefix { get; }
        public string DisplayName => Prefix;
        public string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix.ToUpper());
            CompileArguments(builder);
            return builder.ToString().Trim();
        }

        protected abstract void CompileArguments(StringBuilder builder);
    }
}