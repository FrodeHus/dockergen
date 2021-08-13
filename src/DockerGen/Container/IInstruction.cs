namespace DockerGen.Container
{
    public interface IInstruction
    {
        string DisplayName { get; }
        string Description { get; }
        string Id { get; set; }

        string Compile();
    }
}
