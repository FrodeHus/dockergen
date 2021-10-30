using System;
using System.Text;
using System.Text.Json;

namespace DockerGen.Container
{
    public abstract class Instruction: IDockerInstruction
    {

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public abstract string Description { get; }
        public abstract string Prefix { get; }
        public virtual string DisplayName => Prefix;


        public virtual string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix.ToUpper());
            CompileArguments(builder);
            return builder.ToString().Trim();
        }

        protected abstract void CompileArguments(StringBuilder builder);
    }
}