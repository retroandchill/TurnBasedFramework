using GameDataAccessTools.Core.DataRetrieval;
using Pokemon.Data;
using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.DeveloperSettings;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor;

[UClass(ClassFlags.DefaultConfig, DisplayName = "Pokémon Editor", ConfigCategory = "Game")]
public class UPokemonEditorSettings : UDeveloperSettings
{
    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config, Category = "Pbs")]
    [ClampMin("1")]
    [UIMax("1")]
    [Categories(UItem.PocketCategory)]
    public IReadOnlyDictionary<int, FGameplayTag> PocketNumberToGameplayTag { get; }

    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadWrite | PropertyFlags.Config, Category = "Pbs")]
    [Categories(UEvolutionMethod.TagCategory)]
    public IReadOnlyDictionary<FName, FGameplayTag> EvolutionConditionToGameplayTag { get; }
}