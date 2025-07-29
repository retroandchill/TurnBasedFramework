using UnrealSharp.Attributes;

namespace Pokemon.Core.Evaluator.GrowthRates;

[UClass]
public sealed class UParabolicExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 6 / 5 - 15 * (int)Math.Pow(level, 2) + 100 * level - 140;
    }
}