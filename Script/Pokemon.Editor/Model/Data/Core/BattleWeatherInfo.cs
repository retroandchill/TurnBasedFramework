using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record BattleWeatherInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
}
