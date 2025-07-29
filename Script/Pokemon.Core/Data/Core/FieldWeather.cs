using GameAccessTools.SourceGenerator.Attributes;
using LanguageExt;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Core.Data.Core;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UFieldWeather : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    public Option<FBattleWeatherHandle> BattleWeather { get; init; }
}