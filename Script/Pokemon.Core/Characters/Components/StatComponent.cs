using UnrealSharp.Attributes;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters.Components;

[UClass]
public class UStatComponent : UTurnBasedUnitComponent
{
    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Stats")]
    public int Level { get; private set; }
}