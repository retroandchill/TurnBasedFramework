using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record EnvironmentInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
}
