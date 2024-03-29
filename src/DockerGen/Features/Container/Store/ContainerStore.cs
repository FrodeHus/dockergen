﻿using DockerGen.Container;
using DockerGen.Container.Recipes;
using Fluxor;
using System.Collections.Generic;
using System.Linq;
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
        public bool SidePanelOpen { get; set; }
        [JsonIgnore]
        public int ItemIndex { get; internal set; } = -1;
        [JsonIgnore]
        public BuildStage ActiveStage { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }
    }

    public class ContainerFeature : Feature<ContainerState>
    {

        public override string GetName() => "Container";

        protected override ContainerState GetInitialState()
        {
            return new ContainerState
            {
                Container = new ContainerImage(),
                Recipes = new List<Recipe>()
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
            return state with
            {
                Container = action.State.Container,
                SidePanelOpen = action.State.SidePanelOpen,
            };
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
        public static ContainerState OnDragSetIndex(ContainerState state, DragSetIndexAction action)
        {
            return state with
            {
                ItemIndex = action.ItemIndex
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

        [ReducerMethod(typeof(ContainerPanelOpenAction))]
        public static ContainerState OnSidePanelOpen(ContainerState state)
        {
            return state with
            {
                SidePanelOpen = true
            };
        }
        [ReducerMethod(typeof(ContainerPanelCloseAction))]
        public static ContainerState OnSidePanelClose(ContainerState state)
        {
            return state with
            {
                SidePanelOpen = false
            };
        }

        [ReducerMethod]
        public static ContainerState OnSetActiveStage(ContainerState state, ContainerSetActiveStage action)
        {
            return state with
            {
                ActiveStage = action.Stage
            };
        }

        [ReducerMethod]
        public static ContainerState OnStartDrag(ContainerState state, StartDragAction action)
        {
            return state with
            {
                IsDragging = true,
                CurrentInstruction = action.Instruction
            };
        }
        [ReducerMethod(typeof(EndDragAction))]
        public static ContainerState OnEndDrag(ContainerState state)
        {
            return state with
            {
                IsDragging = false,
                CurrentInstruction = null
            };
        }

        [ReducerMethod]
        public static ContainerState OnRecipesLoaded(ContainerState state, SetRecipesAction action)
		{
            return state with
            {
                Recipes = action.Recipes
            };
		}
    }
}
