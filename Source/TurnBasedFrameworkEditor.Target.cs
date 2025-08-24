// Copyright Epic Games, Inc. All Rights Reserved.

using System.Collections.Generic;
using UnrealBuildTool;

public class TurnBasedFrameworkEditorTarget : TargetRules
{
    public TurnBasedFrameworkEditorTarget(TargetInfo target)
        : base(target)
    {
        Type = TargetType.Editor;
        DefaultBuildSettings = BuildSettingsVersion.V5;
        IncludeOrderVersion = EngineIncludeOrderVersion.Unreal5_6;
        ExtraModuleNames.Add("TurnBasedFramework");
        RegisterModulesCreatedByRider();
    }

    private void RegisterModulesCreatedByRider()
    {
        ExtraModuleNames.AddRange(["PokemonEditorTools"]);
    }
}
