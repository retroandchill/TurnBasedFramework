using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.GameDataAccessTools;

namespace PokemonData.Data.Core;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEnvironment : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public TSoftObjectPtr<UStaticMesh> BattleBase { get; init; }
}