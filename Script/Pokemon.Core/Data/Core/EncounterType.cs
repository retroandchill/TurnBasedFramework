using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Core.Data.Core;

[UEnum]
public enum EEncounterTrigger : byte
{
    Land,
    Cave,
    Water,
    Fishing,
    Contest,
    None
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEncounterType : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Encounters")]
    public EEncounterTrigger Trigger { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Encounters")]
    public int TriggerChance { get; init; }
}