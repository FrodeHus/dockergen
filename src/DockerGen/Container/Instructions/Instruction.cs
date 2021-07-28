using System;
using System.Text;

namespace DockerGen.Container
{
    public abstract class Instruction
    {

        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public event EventHandler<InstructionEventArgs> OnInstructionChanged;
        public abstract string Description { get; }
        protected abstract string Prefix { get; }
        public virtual string DisplayName => Prefix;
        public string Compile()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0} ", Prefix.ToUpper());
            CompileArguments(builder);
            return builder.ToString().Trim();
        }

        protected abstract void CompileArguments(StringBuilder builder);
        protected void FireInstructionChanged()
        {
            OnInstructionChanged?.Invoke(this, new InstructionEventArgs(this));
        }
    }
}