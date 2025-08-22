using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UEnum]
public enum EStatType : byte
{
    Main,
    MainBattle,
    Battle,
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UStat : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Stats";
    public const string MainOnlyCategory = $"{TagCategory}.Main";
    public const string MainBattleCategory = $"{TagCategory}.MainBattle";
    public const string BattleOnlyCategory = $"{TagCategory}.Battle";
    public const string AnyMainCategory = $"{MainOnlyCategory},{MainBattleCategory}";
    public const string AnyBattleCategory = $"{BattleOnlyCategory},{MainBattleCategory}";

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Identification"
    )]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere,
        Category = "Identification"
    )]
    public int RowIndex { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayNameBrief { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    public EStatType StatType { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Metadata",
        DisplayName = "PBS Order"
    )]
    public int PbsOrder { get; init; }

    public bool IsMainStat => StatType is EStatType.Main or EStatType.MainBattle;
    public bool IsBattleStat => StatType is EStatType.Battle or EStatType.MainBattle;
}
