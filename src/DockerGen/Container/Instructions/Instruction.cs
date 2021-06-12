namespace DockerGen.Container
{
    public abstract class Instruction
    {
        protected abstract string Prefix { get; }
        public virtual string Compile()
        {
            return null;
        }
    }
}