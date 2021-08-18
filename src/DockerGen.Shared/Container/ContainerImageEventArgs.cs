namespace DockerGen.Container
{
    public class ContainerImageEventArgs
    {
        public ContainerImageEventArgs(ContainerImage image)
        {
            Image = image;
        }
        public ContainerImage Image { get; set; }
    }
}