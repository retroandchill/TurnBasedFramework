using Microsoft.CodeAnalysis.Options;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record FieldWeatherInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public FGameplayTag? BattleWeather { get; init; }
}
