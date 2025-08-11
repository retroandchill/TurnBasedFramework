using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record EvolutionMethodInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public TSubclassOf<UEvolutionConditionData> ConditionType { get; init; }
}