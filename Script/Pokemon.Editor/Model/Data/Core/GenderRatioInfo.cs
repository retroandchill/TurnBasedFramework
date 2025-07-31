using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record GenderRatioInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    
    public required ESpecialGenderRatio SpecialGenderRatio { get; init; }
    
    public byte? FemaleChance { get; init; }
}
