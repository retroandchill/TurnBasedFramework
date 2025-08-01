﻿using UnrealBuildTool;

public class GameDataAccessToolsEditor : ModuleRules
{
    public GameDataAccessToolsEditor(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                "UnrealSharpCore",
                "UnrealEd",
                "GameDataAccessTools",
            }
        );

        PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "CoreUObject",
                "UnrealSharpBinds",
                "Engine",
                "Slate",
                "SlateCore",
                "WorkspaceMenuStructure",
                "InputCore", 
                "UnrealSharpProcHelper",
                "JsonUtilities",
                "PropertyPath",
                "ToolWidgets",
                "GameplayTags",
                "GameplayTagsEditor"
            }
        );
    }
}
