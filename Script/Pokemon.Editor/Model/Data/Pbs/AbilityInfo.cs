using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record AbilityInfo
{
    public required FGameplayTag Id { get; init; }
    public int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    public required FText Description { get; init; }

    public FGameplayTagContainer Tags { get; init; } = new()
    {
        GameplayTags = [],
        ParentTags = []
    };
}
