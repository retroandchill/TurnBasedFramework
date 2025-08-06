using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.DeveloperSettings;

namespace GameDataAccessTools.Core;

[UClass(ClassFlags.DefaultConfig, DisplayName = "Game Data Access Tools", ConfigCategory = "Game")]
public class UGameDataAccessToolsSettings : UDeveloperSettings
{
    #if WITH_EDITOR
    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config, Category = "Serializers")]
    public FName NewGameplayTagsPath { get; }
    #endif
}