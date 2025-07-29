using GameAccessTools.SourceGenerator.Attributes;
using Pokemon.Core.Evaluator.GrowthRates;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.GameDataAccessTools;

namespace Pokemon.Core.Data.Core;

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