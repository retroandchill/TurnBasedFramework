using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public readonly record struct IntBounds(int Min, int Max);

public record BerryPlantInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public int HoursPerStage { get; init; } = 3;
    public int DryingPerHour { get; init; } = 15;
    public IntBounds Yield { get; init; }
}
