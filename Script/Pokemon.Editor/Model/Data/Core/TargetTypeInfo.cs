using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Core;

public record TargetTypeInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    
    public required ETargetCount NumTargets { get; init; }

    public required bool TargetsFoes { get; init; }
    
    public required bool TargetsAll { get; init; }
    
    public required  bool AffectsFoeSide { get; init; }
    
    public required bool LongRange { get; init; }
}
