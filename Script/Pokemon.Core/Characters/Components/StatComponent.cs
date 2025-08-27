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
    [field: ClampMin("0")]
    [field: UIMin("0")]
    [field: ClampMax("31")]
    [field: UIMax("31")]
    public int IV { get; init; }

    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    public bool IVMaxed { get; init; }

    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [field: ClampMin("0")]
    [field: UIMin("0")]
    [field: ClampMax("252")]
    [field: UIMax("252")]
    public int EV { get; init; }
}

public readonly record struct FStatEntry(
    [field: UProperty(PropertyFlags.BlueprintReadOnly)] int CurrentValue,
    [field: UProperty(PropertyFlags.BlueprintReadOnly)] FStatData Data
);

[UStruct]
public readonly record struct FStatChange(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)] int Before,
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)] int After
)
{
    public int Difference => After - Before;
}

[UStruct]
public readonly record struct FExpPercentChange(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)] float Before,
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)] float After
);

[UStruct]
public readonly record struct FLevelUpStatChanges(
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
        FStatChange LevelChange,
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
        FExpPercentChange ExpPercentChange,
    [field: UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
        IReadOnlyDictionary<FGameplayTag, FStatChange> StatChanges
);

[UClass]
public class UStatComponent : UTurnBasedUnitComponent
{
    public static readonly FGameplayTag StatHP = GameplayTags.Pokemon_Data_Stats_Main_HP;
    public static readonly FGameplayTag StatAttack =
        GameplayTags.Pokemon_Data_Stats_MainBattle_ATTACK;
    public static readonly FGameplayTag StatDefense =
        GameplayTags.Pokemon_Data_Stats_MainBattle_DEFENSE;
    public static readonly FGameplayTag StatSpAtk =
        GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_ATTACK;
    public static readonly FGameplayTag StatSpDef =
        GameplayTags.Pokemon_Data_Stats_MainBattle_SPECIAL_DEFENSE;
    public static readonly FGameplayTag StatSpeed =
        GameplayTags.Pokemon_Data_Stats_MainBattle_SPEED;

    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Stats")]
    public int Level { get; private set; }

    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Exp")]
    public int Exp { get; private set; }

    public int ExpForNextLevel
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Exp")]
        get
        {
            if (Level == UGrowthRate.MaxLevel)
                return 0;

            return GetGameInstanceSubsystem<UPokemonSubsystem>()
                .GetExpGrowthFormula(Pokemon.SpeciesData.GrowthRate)
                .GetMinimumExpForLevel(Level + 1);
        }
    }

    public float ExpPercent
    {
        [method: UFunction(FunctionFlags.BlueprintPure, Category = "Exp")]
        get
        {
            if (Level == UGrowthRate.MaxLevel)
                return 0.0f;

            var growthRate = GetGameInstanceSubsystem<UPokemonSubsystem>()
                .GetExpGrowthFormula(Pokemon.SpeciesData.GrowthRate);
            var expNeededForLevel = growthRate.GetMinimumExpForLevel(Level);
            var totalNeededForLevel = growthRate.GetMinimumExpForLevel(Level + 1);
            return (float)(Exp - expNeededForLevel) / totalNeededForLevel;
        }
    }

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
    public FStatData GetStat([Categories(UStat.AnyMainCategory)] FGameplayTag stat)
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
        var nature = NatureOverride.Match(
            x => GameData.Natures.GetEntry(x),
            () => GameData.Natures.GetEntry(Nature)
        );
        foreach (var (id, stat) in Stats)
        {
            var baseValue = species.BaseStats[id];
            Stats[id] = stat with
            {
                CurrentValue = CalculateStatValue(id, baseValue, nature, stat.Data),
            };
        }
    }

    [UFunction(FunctionFlags.BlueprintEvent, Category = "Stats")]
    protected virtual int CalculateStatValue(
        FGameplayTag stat,
        int baseValue,
        UNature nature,
        in FStatData data
    )
    {
        var statData = GameData.Stats.GetEntry(stat);
        if (statData.StatType == EStatType.Main)
        {
            return (2 * baseValue + data.IV + data.EV / 4) * Level / 100 + Level + 10;
        }

        var natureModifer = nature.StatMultipliers.GetValueOrDefault(stat, 100);
        return ((2 * baseValue + data.IV + data.EV / 4) * Level / 100 + 5) * natureModifer / 100;
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "Stats")]
    public async ValueTask<FLevelUpStatChanges> GainExp(int change, bool showMessages = false)
    {
        var levelBefore = Level;
        var expBefore = ExpPercent;
        var statsBefore = Stats.ToDictionary(x => x.Key, x => x.Value.CurrentValue);

        Exp += change;

        while (Exp >= ExpForNextLevel)
        {
            Level++;
        }

        var update = new FLevelUpStatChanges(
            new FStatChange(levelBefore, Level),
            new FExpPercentChange(expBefore, ExpPercent),
            statsBefore.ToDictionary(
                x => x.Key,
                x => new FStatChange(x.Value, Stats[x.Key].CurrentValue)
            )
        );

        if (Level <= levelBefore)
            return update;
        RecalculateStats();

        var hpDiff = update.StatChanges[StatHP].Difference;
        CurrentHP += hpDiff;

        if (showMessages)
        {
            await Pokemon.DisplayLevelUp(update);
        }

        return update;
    }
}
