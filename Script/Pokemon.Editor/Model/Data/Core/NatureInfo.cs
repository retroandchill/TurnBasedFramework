using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public readonly record struct NatureStatMultiplier(FGameplayTag Stat, int Change);

public record NatureInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public required IReadOnlyList<NatureStatMultiplier> StatMultipliers { get; init; }
}
