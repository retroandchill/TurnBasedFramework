using Pokemon.Core.Characters.Components;
using UnrealSharp.Attributes;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters;

[UClass(ClassFlags.Abstract)]
public class UPokemon : UTurnBasedUnit
{
    [UProperty(PropertyFlags.BlueprintReadOnly)]
    public UIdentityComponent IdentityComponent { get; private set; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly)]
    public UStatComponent StatComponent { get; private set; }
}