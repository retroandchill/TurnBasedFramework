using LanguageExt;
using Pokemon.Data;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.GameplayTags;
using UnrealSharp.TurnBasedCore;

namespace Pokemon.Core.Characters.Components;

[UStruct]
public readonly record struct FStatData
{
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [ClampMin("0")]
    [UIMin("0")]
    [ClampMax("31")]
    [UIMax("31")]
    public int IV { get; init; }

    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    public bool IVMaxed { get; init; }
    
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [ClampMin("0")]
    [UIMin("0")]
    [ClampMax("252")]
    [UIMax("252")]
    public int EV { get; init; }
}

public readonly record struct FStatEntry(
    [field: UProperty(PropertyFlags.BlueprintReadOnly)]
    int CurrentValue,
    [field: UProperty(PropertyFlags.BlueprintReadOnly)]
    FStatData Data);


[UClass]
public class UStatComponent : UTurnBasedUnitComponent
{
    public static readonly FGameplayTag StatHP = GameplayTags.Pokemon_Data_Stats_Main_HP;
    public static readonly FGameplayTag StatAttack = GameplayTags.Pokemon_Data_Stats_MainBattle_ATTACK;
    public static readonly FGameplayTag StatDefense = GameplayTags.Pokemon_Data_Stats_MainBattle_DEFENSE;
    public static readonly FGameplayTag StatSpAtk = GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_ATTACK;
    public static readonly FGameplayTag StatSpDef = GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_DEFENSE;
    public static readonly FGameplayTag StatSpeed = GameplayTags.Pokemon_Data_Stats_MainBattle_SPEED;
    
    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Stats")]
    public int Level { get; private set; }

    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Stats")]
    public int Exp { get; private set; }
    
    [UProperty]
    private IDictionary<FGameplayTag, FStatEntry> Stats { get; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public FGameplayTag Nature { get; set; }
    
    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public Option<FGameplayTag> NatureOverride { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite)]
    public int CurrentHP
    {
        get;
        set => field = Math.Clamp(value, 0, MaxHP);
    }

    public int MaxHP
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatHP].CurrentValue;
    }
    
    public int Attack
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatAttack].CurrentValue;
    }
    
    public int Defense
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatDefense].CurrentValue;
    }
    
    public int SpecialAttack
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatSpAtk].CurrentValue;
    }
    
    public int SpecialDefense
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatSpDef].CurrentValue;
    }
    
    
    public int Speed
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
        get => Stats[StatSpeed].CurrentValue;
    }

    public UPokemon Pokemon
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Components")]
        get => (UPokemon)OwningUnit;
    }

    [UFunction(FunctionFlags.BlueprintPure, Category = "Stats")]
    public FStatData GetStat([Categories(UStat.AnyMainCategory)]FGameplayTag stat)
    {
        return Stats[stat].Data;
    }
    
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Stats")]
    public void UpdateStat([Categories(UStat.AnyMainCategory)] FGameplayTag stat, FStatData data)
    {
        Stats[stat] = Stats[stat] with { Data = data };
        RecalculateStats();
    }
    
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Stats")]
    public void RecalculateStats()
    {
        var species = Pokemon.SpeciesData;
        var nature = NatureOverride.Match(x => GameData.Natures.GetEntry(x), 
            () => GameData.Natures.GetEntry(Nature));
        foreach (var (id, stat) in Stats)
        {
            var baseValue = species.BaseStats[id];
            Stats[id] = stat with { CurrentValue = CalculateStatValue(id, baseValue, nature, stat.Data) };
        }
    }

    [UFunction(FunctionFlags.BlueprintEvent, Category = "Stats")]
    protected virtual int CalculateStatValue(FGameplayTag stat, int baseValue, UNature nature, in FStatData data)
    {
        var statData = GameData.Stats.GetEntry(stat);
        if (statData.StatType == EStatType.Main)
        {
            return (2 * baseValue + data.IV + data.EV / 4) * Level / 100 + Level + 10;
        }

        var natureModifer = nature.StatMultipliers.GetValueOrDefault(stat, 100);
        return ((2 * baseValue + data.IV + data.EV / 4) * Level / 100 + 5) * natureModifer / 100;
    }
}