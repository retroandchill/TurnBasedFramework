using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record TypeInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public required bool IsSpecialType { get; init; }
    public required bool IsPseudoType { get; init; }
    public required FGameplayTagContainer Weaknesses { get; init; }
    public required FGameplayTagContainer Resistances { get; init; }
    public required FGameplayTagContainer Immunities { get; init; }
    public required FGameplayTagContainer Tags { get; init; }
}
