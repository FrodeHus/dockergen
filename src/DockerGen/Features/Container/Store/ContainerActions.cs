using DockerGen.Container;

namespace DockerGen.Features.Container.Store
{
	public record ContainerAddBuildStageAction(BuildStage Stage);

	public record ContainerRemoveBuildStageAction(BuildStage Stage);
	public record ContainerSetStateAction(ContainerState State);
	public record ContainerLoadStateAction();
	public record ContainerLoadStateSuccessAction();
	public record ContainerLoadStateFailureAction(string ErrorMessage);
	public record ContainerPersistStateAction(ContainerState State);
	public record ContainerPersistStateSuccessAction();
	public record ContainerPersistStateFailureAction(string ErrorMessage);
	public record ContainerClearStateAction();
	public record ContainerClearStateSuccessAction();
	public record ContainerClearStateFailureAction(string ErrorMessage);
}
