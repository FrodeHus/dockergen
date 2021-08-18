namespace DockerGen.Container
{
    public class BuildStageEventArgs
    {
        public BuildStageEventArgs(BuildStage stage)
        {
            Stage = stage;
        }
        public BuildStage Stage { get; set; }
    }
}