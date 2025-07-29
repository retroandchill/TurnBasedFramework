using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

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
public class UStat : UGameDataEntry
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayNameBrief { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    public EStatType StatType { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata", DisplayName = "PBS Order")]
    public int PbsOrder { get; init; }
}

[UStruct]
[DataHandle]
public readonly partial record struct FMainStatHandle(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    FName Id) : IDataHandle<FName, UStat>
{
    public static IDataRepository<FName, UStat> Repository => FStatHandle.Repository;
    
    public static IEnumerable<FName> EntryKeys => GameData.Stats.Entries
        .Where(x => x.Value.StatType is EStatType.Main or EStatType.MainBattle)
        .Select(x => x.Key);
    
    public bool IsValid => Entry is not null;

    public UStat? Entry
    {
        get
        {
            var entry = new FStatHandle(Id).Entry;
            return entry?.StatType is EStatType.Main or EStatType.MainBattle ? entry : null;
        }
    }
}

[UStruct]
[DataHandle]
public readonly partial record struct FMainBattleStatHandle(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    FName Id) : IDataHandle<FName, UStat>
{
    public static IDataRepository<FName, UStat> Repository => FStatHandle.Repository;
    
    public static IEnumerable<FName> EntryKeys => GameData.Stats.Entries
        .Where(x => x.Value.StatType is EStatType.MainBattle)
        .Select(x => x.Key);
    
    public bool IsValid => Entry is not null;

    public UStat? Entry
    {
        get
        {
            var entry = new FStatHandle(Id).Entry;
            return entry?.StatType is EStatType.MainBattle ? entry : null;
        }
    }
}

[UStruct]
[DataHandle]
public readonly partial record struct FBattleStatHandle(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    FName Id) : IDataHandle<FName, UStat>
{
    public static IDataRepository<FName, UStat> Repository => FStatHandle.Repository;
    
    public static IEnumerable<FName> EntryKeys => GameData.Stats.Entries
        .Where(x => x.Value.StatType is EStatType.MainBattle or EStatType.Battle)
        .Select(x => x.Key);
    
    public bool IsValid => Entry is not null;

    public UStat? Entry
    {
        get
        {
            var entry = new FStatHandle(Id).Entry;
            return entry?.StatType is EStatType.MainBattle or EStatType.Battle ? entry : null;
        }
    }
}