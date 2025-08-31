// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;

public class TurnBasedUI : ModuleRules
{
    public TurnBasedUI(ReadOnlyTargetRules target)
        : base(target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicIncludePaths.AddRange(
            [
                // ... add public include paths required here ...
            ]
        );

        PrivateIncludePaths.AddRange(
            [
                // ... add other private include paths required here ...
            ]
        );

        PublicDependencyModuleNames.AddRange(
            [
                "Core",
                "CommonUI",
                "UnrealSharpCore",
                "UnrealSharpBinds",
                "UnrealSharpAsync",
                "CommonInput",
                // ... add other public dependencies that you statically link with here ...
            ]
        );

        PrivateDependencyModuleNames.AddRange(
            [
                "CoreUObject",
                "Engine",
                "Slate",
                "SlateCore",
                "CommonUtilities",
                "UMG",
                "GameplayTags",
                "EnhancedInput",
                "DeveloperSettings"
                // ... add private dependencies that you statically link with here ...
            ]
        );

        DynamicallyLoadedModuleNames.AddRange(
            [
                // ... add any modules that your module loads dynamically here ...
            ]
        );
    }
}
