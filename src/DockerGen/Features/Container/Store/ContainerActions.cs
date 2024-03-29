﻿using DockerGen.Container;
using DockerGen.Container.Recipes;
using System.Collections.Generic;

namespace DockerGen.Features.Container.Store
{
    public record ContainerAddBuildStageAction(BuildStage Stage);

    public record ContainerRemoveBuildStageAction(BuildStage Stage);
    public record ContainerAddInstructionToStageAction(BuildStage Stage, IInstruction Instruction, int Index = -1);
    public record ContainerRemoveInstructionAction(IInstruction Instruction);
    public record ContainerLoadDockerfileFromStringAction(string dockerfile);
    public record ContainerLoadDockerfileFromStringFailureAction(string ErrorMessage);
    public record ContainerLoadDockerfileFromStringSuccessAction();
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
    public record DragAction(bool IsDragging);
    public record DragSetIndexAction(int ItemIndex = -1);
    public record ContainerSetCurrentInstructionAction(IInstruction Instruction);
    public record StartDragAction(IInstruction Instruction);
    public record EndDragAction();
    public record ContainerUpdatedAction();    
    public record ContainerPanelOpenAction();
    public record ContainerPanelCloseAction();
    public record ContainerSetActiveStage(BuildStage Stage);
    public record ContainerLoadQuickLinkAction(string QuickLinkId);
    public record ContainerLoadQuickLinkSuccess();
    public record ContainerLoadQuickLinkFailed(string ErrorMessage);
    public record LoadRecipesAction();
    public record SetRecipesAction(IEnumerable<Recipe> Recipes);
}
