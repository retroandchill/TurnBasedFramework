using Pokemon.Core.Characters.Components;
using UnrealSharp.Attributes;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters;

[UClass(ClassFlags.Abstract)]
public class UPokemon : UTurnBasedUnit
{
    [UProperty(PropertyFlags.EditDefaultsOnly | PropertyFlags.BlueprintReadOnly | PropertyFlags.Instanced, Category = "Components")]
    public UStatComponent Stats { get; }
    
    protected override void InitializeComponents()
    {
        RegisterNewComponent(Stats);
    }
}