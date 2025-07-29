using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

public abstract class GrowthRate
{
    public static int MaxLevel => UObject.GetDefault<UGameDataSettings>().MaxLevel;
    
    public abstract FGameplayTag Key { get; }
    
    public abstract int GetMinimumExpForLevel(int level);
    
    public int MaximumExp => GetMinimumExpForLevel(MaxLevel);

    public int AddExp(int exp1, int exp2) => Math.Clamp(exp1 + exp2, 0, MaximumExp);

    public int GetLevelForExp(int exp)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(exp, 0);
        var max = MaxLevel;
        if (exp >= MaximumExp) return max;

        for (var i = 0; i <= max; i++)
        {
            if (exp < GetMinimumExpForLevel(i)) return i - 1;
        }

        return max;
    }
}

public sealed class MediumExpGrowth : GrowthRate
{
    public override FGameplayTag Key { get; }

    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3);
    }
}

public class GrowthRateContainer(IEnumerable<GrowthRate> growthRates)
{
    private readonly Dictionary<FGameplayTag, GrowthRate> _growthRates = growthRates.ToDictionary(x => x.Key);
    
    public GrowthRate GetGrowthRate(FGameplayTag key)
    {
        return _growthRates[key];
    }

    public bool TryGetGrowthRate(FGameplayTag key, [NotNullWhen(true)] out GrowthRate? growthRate)
    {
        return _growthRates.TryGetValue(key, out growthRate);
    }
}

[UClass(ClassFlags.Abstract)]
public class UGrowthRateFormula : UObject
{
    [UFunction(FunctionFlags.BlueprintEvent)] 
    public virtual int GetMinimumExpForLevel(int level) 
    {
        throw new NotImplementedException();
    }
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UGrowthRate : UGameDataEntry
{
    public static int MaxLevel => GetDefault<UGameDataSettings>().MaxLevel;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Calculation")]
    public TSubclassOf<UGrowthRateFormula> Formula { get; init; }

    [UFunction(FunctionFlags.BlueprintPure, Category = "Calculation")]
    public int GetMinimumExpForLevel(int level)
    {
        return Formula.DefaultObject.GetMinimumExpForLevel(level);
    }

    public int MaximumExp
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Calculation")]
        get => GetMinimumExpForLevel(MaxLevel);
    }
    
    public int AddExp(int exp1, int exp2) => Math.Clamp(exp1 + exp2, 0, MaximumExp);

    public int GetLevelForExp(int exp)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(exp, 0);
        var max = MaxLevel;
        if (exp >= MaximumExp) return max;

        for (var i = 0; i <= max; i++)
        {
            if (exp < GetMinimumExpForLevel(i)) return i - 1;
        }

        return max;
    }
}

[UClass]
public sealed class UMediumExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3);
    }
}

[UClass]
public sealed class UErraticExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level switch
        {
            1 => 0,
            <= 50 => (int)Math.Pow(level, 3) * (100 - level) / 50,
            <= 68 => (int)Math.Pow(level, 3) * (150 - level) / 100,
            <= 98 => (int)Math.Pow(level, 3) * ((1911 - 10 * level) / 3) / 500,
            _ => (int)Math.Pow(level, 3) * (160 - level) / 100
        };
    }
}

[UClass]
public sealed class UFluctuatingExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level switch
        {
            1 => 0,
            <= 15 => (int)Math.Pow(level, 3) * (24 + (level + 1) / 3) / 50,
            <= 35 => (int)Math.Pow(level, 3) * (14 + level) / 50,
            _ => (int)Math.Pow(level, 3) * (32 + level / 2) / 50
        };
    }
}

[UClass]
public sealed class UParabolicExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 6 / 5 - 15 * (int)Math.Pow(level, 2) + 100 * level - 140;
    }
}

[UClass]
public sealed class UFastExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 4 / 5;
    }
}

[UClass]
public sealed class USlowExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 5 / 4;
    }
}