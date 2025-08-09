using System.ComponentModel.DataAnnotations;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record TypeInfo
{
    [PbsKey]
    [PbsGameplayTag(UType.TagCategory, Create = true)]
    public required FGameplayTag Id { get; init; }
    
    [PbsIndex]
    public int RowIndex { get; init; }
    
    [PbsName("Name")]
    public required FText DisplayName { get; init; }
    public bool IsSpecialType { get; init; }
    public bool IsPseudoType { get; init; }

    [PbsGameplayTag(UType.TagCategory)] 
    public IReadOnlyList<FName> Weaknesses { get; init; } = [];
    
    [PbsGameplayTag(UType.TagCategory)]

    public IReadOnlyList<FName> Resistances { get; init; } = [];

    [PbsGameplayTag(UType.TagCategory)] 
    public IReadOnlyList<FName> Immunities { get; init; } = [];
    
    [PbsName("Flags")]
    [PbsGameplayTag(UType.MetadataCategory, Create = true)]
    public FGameplayTagContainer Tags { get; init; }
}
