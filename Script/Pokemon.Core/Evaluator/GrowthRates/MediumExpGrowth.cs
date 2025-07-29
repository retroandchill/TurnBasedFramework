using UnrealSharp.Attributes;

namespace Pokemon.Core.Evaluator.GrowthRates;

[UClass]
public sealed class UMediumExpGrowth : UGrowthRateFormula
{
    public override int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3);
    }
}