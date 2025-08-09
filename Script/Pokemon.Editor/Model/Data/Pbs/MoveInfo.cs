using Pokemon.Data.Core;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record MoveInfo
{
    [PbsKey]
    [PbsGameplayTag(UMove.TagCategory, Create = true)]
    public required FGameplayTag Id { get; init; }
    
    [PbsIndex]
    public int RowIndex { get; init; }
    
    [PbsName("Name")]
    public FText DisplayName { get; init; } = "Unnamed";

    public FText Description { get; init; } = "???";
    
    
    [PbsGameplayTag(UType.TagCategory)]
    public FGameplayTag Type { get; init; }
    
    public EDamageCategory Category { get; init; } = EDamageCategory.Status;
    
    public int Power { get; init; }
    
    public int Accuracy { get; init; } = 100;
    
    public int TotalPP { get; init; } = 5;
    
    public int Priority { get; init; }
    
    [PbsGameplayTag(UTargetType.TagCategory)]
    public FGameplayTag Target { get; init; }
    
    
    [PbsGameplayTag(UMove.FunctionCodeCategory, Create = true)]
    public FGameplayTag FunctionCode { get; init; }
    
    public int EffectChance { get; init; }
    
    [PbsName("Flags")]
    [PbsGameplayTag(UMove.MetadataCategory, Create = true)]
    public FGameplayTagContainer Tags { get; init; } = new()
    {
        GameplayTags = [],
        ParentTags = []
    };
}
