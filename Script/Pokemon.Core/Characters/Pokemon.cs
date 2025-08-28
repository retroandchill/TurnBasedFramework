using Pokemon.Core.Characters.Components;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.GameplayTags;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters;

[UClass]
public partial class UPokemon : UTurnBasedUnit
{
    public UTrainer Trainer
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Pokémon")]
        get => (UTrainer)SystemLibrary.GetOuterObject(this);
    }
    
    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Instanced, Category = "Components")]
    public UIdentityComponent IdentityComponent { get; private set; }

    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Instanced, Category = "Components")]
    public UStatComponent StatComponent { get; private set; }

    public static UPokemon Create(FGameplayTag species, int level, UTrainer? owner = null)
    {
        return Create(owner ?? PokemonStatics.Player, GetDefault<UPokemonCoreSettings>().PokemonClass, p => p.Initialize(species, level));
    }

    protected virtual void Initialize(FGameplayTag species, int level)
    {
        IdentityComponent.Initialize(species);
        StatComponent.Initialize(level);
    }

    protected override void CreateComponents()
    {
        var settings = GetDefault<UPokemonCoreSettings>();
        IdentityComponent = RegisterNewComponent(settings.IdentityComponentClass);
        StatComponent = RegisterNewComponent(settings.StatComponentClass);

        foreach (var componentClass in settings.AdditionalComponents)
        {
            RegisterNewComponent(componentClass);
        }
    }
}