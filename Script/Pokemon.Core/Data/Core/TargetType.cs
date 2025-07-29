using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Core.Data.Core;

[UEnum]
public enum ETargetCount : byte
{
    NoTarget,
    SingleTarget,
    MultipleTargets
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UTargetType : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public ETargetCount NumTargets { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public bool TargetsFoes { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public bool TargetsAll { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public bool AffectsFoeSide { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public bool LongRange { get; init; }
}