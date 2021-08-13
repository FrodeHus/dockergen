using DockerGen.Container;
using DockerGen.Container.Recipes;
using Fluxor;
using System.Text.Json.Serialization;

namespace DockerGen.Features.Container.Store
{
    public record ContainerState
    {
        public ContainerImage Container { get; init; }
        [JsonIgnore]
        public bool IsDragging { get; set; }
        [JsonIgnore]
        public IInstruction CurrentInstruction { get; set; }
        [JsonIgnore]
        public List<Recipe> Recipes { get; set; }
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

        [ReducerMethod]
        public static ContainerState OnRemoveInstruction(ContainerState state, ContainerRemoveInstructionAction action)
        {
            state.Container.RemoveInstruction(action.Instruction);
            return state with
            {
                Container = state.Container
            };
        }

        [ReducerMethod]
        public static ContainerState OnAddInstruction(ContainerState state, ContainerAddInstructionToStageAction action)
        {
            state.Container.Stages.SingleOrDefault(s => s == action.Stage)?.AddInstruction(action.Instruction, action.Index);
            return state with
            {
                Container = state.Container
            };
        }

        [ReducerMethod]
        public static ContainerState OnDragAction(ContainerState state, DragAction action)
        {
            return state with
            {
                IsDragging = action.IsDragging
            };
        }

        [ReducerMethod]
        public static ContainerState OnSetCurrentInstruction(ContainerState state, ContainerSetCurrentInstructionAction action)
        {
            return state with
            {
                CurrentInstruction = action.Instruction
            };
        }

        [ReducerMethod]
        public static ContainerState OnRecipesLoaded(ContainerState state, ContainerRecipesLoadedAction action)
        {
            return state with
            {
                Recipes = action.Recipes
            };
        }
    }
}
