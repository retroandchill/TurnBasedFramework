using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record AbilityInfo
{
    [PbsKey]
    [PbsGameplayTag(UAbility.TagCategory, Create = true)]
    public required FGameplayTag Id { get; init; }

    [PbsIndex]
    public int RowIndex { get; init; }

    [PbsName("Name")]
    [PbsLocalizedText("PokemonAbilities", "{0}_DisplayName")]
    public FText DisplayName { get; init; } = "Unnamed";

    [PbsLocalizedText("PokemonAbilities", "{0}_Description")]
    public FText Description { get; init; } = "???";

    [PbsName("Flags")]
    [PbsGameplayTag(UAbility.MetadataCategory, Create = true)]
    public FGameplayTagContainer Tags { get; init; } = new() { GameplayTags = [], ParentTags = [] };
}
