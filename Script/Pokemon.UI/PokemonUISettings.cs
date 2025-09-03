using JetBrains.Annotations;
using Pokemon.UI.Modals;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.DeveloperSettings;

namespace Pokemon.UI;

[UClass(ClassFlags.DefaultConfig, DisplayName = "Pokémon UI", ConfigCategory = "Game")]
public class UPokemonUISettings : UDeveloperSettings
{
    [UProperty(
        PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config,
        Category = "Classes"
    )]
    [UsedImplicitly]
    public TSoftClassPtr<UMessageDisplayScreen> MessageDisplayScreen { get; }
}