using UnrealSharp.Attributes;

namespace Pokemon.Core.Evaluator.GrowthRates;

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