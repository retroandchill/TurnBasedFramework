using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UEnum]
public enum EStatType : byte
{
    Main,
    MainBattle,
    Battle
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UStat : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Core.Stat";
    public const string MainOnlyCategory = "Pokemon.Data.Core.Stat.Main";
    public const string MainBattleCategory = "Pokemon.Data.Core.Stat.MainBattle";
    public const string BattleOnlyCategory = "Pokemon.Data.Core.Stat.Battle";
    public const string AnyMainCategory = "Pokemon.Data.Core.Stat.Main,Pokemon.Data.Core.Stat.MainBattle";
    public const string AnyBattleCategory = "Pokemon.Data.Core.Stat.Battle,Pokemon.Data.Core.Stat.MainBattle";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [UMetaData("Categories", TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayNameBrief { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    public EStatType StatType { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata", DisplayName = "PBS Order")]
    public int PbsOrder { get; init; }
}