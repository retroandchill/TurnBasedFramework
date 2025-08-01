using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record EncounterTypeInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public FGameplayTag? Trigger { get; init; }
    public required int TriggerChance { get; init; }
}
