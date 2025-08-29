using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record TrainerTypeInfo
{
    [PbsKey]
    [PbsGameplayTag(UTrainerType.TagCategory, Create = true, Separator = "_")]
    public required FGameplayTag Id { get; init; }

    [PbsIndex]
    public int RowIndex { get; init; }

    [PbsName("Name")]
    [PbsLocalizedText("PokemonTrainerType", "{0}_DisplayName")]
    public FText DisplayName { get; init; } = "Unnamed";

    public ETrainerGender Gender { get; init; } = ETrainerGender.Unknown;
    
    [PbsRange<int>(0)]
    public int BasePayout { get; init; } = 30;
    
    [PbsRange<int>(0)]
    public int? SkillLevel { get; init; }
    
    [PbsName("Flags")]
    [PbsGameplayTag(UTrainerType.MetadataCategory, Create = true, Separator = "_")]
    public FGameplayTagContainer Tags { get; init; }
}