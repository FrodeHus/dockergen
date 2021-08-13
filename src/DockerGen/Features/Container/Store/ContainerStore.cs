using DockerGen.Container;
using Fluxor;

namespace DockerGen.Features.Container.Store
{
	public record ContainerState
	{
		public ContainerImage Container { get; init; }
	}

	public class ContainerFeature : Feature<ContainerState>
	{

		public override string GetName() => "Container";

		protected override ContainerState GetInitialState()
		{
			return new ContainerState
			{
				Container = new ContainerImage()
			};
		}
	}


	public static class ContainerReducers
	{
		[ReducerMethod]
		public static ContainerState OnAddBuildStage(ContainerState state, ContainerAddBuildStageAction action)
		{
			state.Container.AddStage(action.Stage);
			return state with
			{
				Container = state.Container
			};
		}

		[ReducerMethod]
		public static ContainerState OnRemoveBuildStage(ContainerState state, ContainerRemoveBuildStageAction action)
		{
			state.Container.RemoveStage(action.Stage);
			return state with
			{
				Container = state.Container
			};
		}

		[ReducerMethod]
		public static ContainerState OnSetState(ContainerState state, ContainerSetStateAction action)
		{
			return action.State;
		}
	}
}
