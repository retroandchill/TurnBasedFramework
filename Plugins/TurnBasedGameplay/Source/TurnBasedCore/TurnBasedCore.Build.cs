using UnrealBuildTool;

public class TurnBasedCore : ModuleRules
{
    public TurnBasedCore(ReadOnlyTargetRules Target)
        : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new string[] { "Core", "UnrealSharpCore", "CommonUtilities", "CoreUObject" }
        );

        PrivateDependencyModuleNames.AddRange(
            new string[] { "CoreUObject", "Engine", "Slate", "SlateCore", "UnrealSharpBinds" }
        );
    }
}
