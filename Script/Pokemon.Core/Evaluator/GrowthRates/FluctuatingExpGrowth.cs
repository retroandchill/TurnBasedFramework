using UnrealSharp.Attributes;

namespace Pokemon.Core.Evaluator.GrowthRates;

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