using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Data.Core;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEvolutionMethod : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    // TODO: Figure out how to handle the evaluation of the pokemon information
}
