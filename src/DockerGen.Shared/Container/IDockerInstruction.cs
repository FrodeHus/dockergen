namespace DockerGen.Container
{
    public interface IDockerInstruction : IInstruction
    {
        string Prefix { get; }
    }
}
