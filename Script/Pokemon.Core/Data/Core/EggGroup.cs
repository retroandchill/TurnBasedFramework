using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Core.Data.Core;

[UEnum]
public enum EEggGroupType : byte
{
    WithSameEggGroup,
    WithAnyEggGroup,
    WithNoEggGroups
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEggGroup : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Breeding")]
    public EEggGroupType BreedingType { get; init; }
}