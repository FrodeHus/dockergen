namespace DockerGen.Container
{
    public interface IInstruction
    {
        string DisplayName { get; }
        string Description { get; }
        string Id { get; set; }
        /// <summary>
        /// Type of UI component to render this instruction
        /// </summary>
        Type UIType { get; }

        string Compile();
    }
}
