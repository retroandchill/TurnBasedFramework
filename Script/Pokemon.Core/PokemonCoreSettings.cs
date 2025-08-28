using Pokemon.Core.Characters;
using Pokemon.Core.Characters.Components;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.DeveloperSettings;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core;

[UClass(ClassFlags.DefaultConfig, DisplayName = "Pokémon Core", ConfigCategory = "Game")]
public class UPokemonCoreSettings : UDeveloperSettings
{
    [UProperty(
        PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config,
        Category = "DefaultClasses"
    )]
    public TSubclassOf<UPokemon> PokemonClass { get; }
    
    [UProperty(
        PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config,
        Category = "DefaultClasses|Components"
    )]
    public TSubclassOf<UIdentityComponent> IdentityComponentClass { get; }
    
    [UProperty(
        PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config,
        Category = "DefaultClasses|Components"
    )]
    public TSubclassOf<UStatComponent> StatComponentClass { get; }
    
    [UProperty(
        PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Config,
        Category = "DefaultClasses|Components"
    )]
    public IReadOnlyList<TSubclassOf<UTurnBasedUnitComponent>> AdditionalComponents { get; }

    public UPokemonCoreSettings()
    {
        PokemonClass = typeof(UPokemon);
        IdentityComponentClass = typeof(UIdentityComponent);
        StatComponentClass = typeof(UStatComponent);
        AdditionalComponents = new List<TSubclassOf<UTurnBasedUnitComponent>>();
    }
}