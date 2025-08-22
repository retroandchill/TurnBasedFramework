using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class ErraticExpGrowthFormula : IExpGrowthFormula
{
    public FGameplayTag GrowthRateFor => GameplayTags.Pokemon_Data_GrowthRates_Erratic;

    public int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level switch
        {
            1 => 0,
            <= 50 => (int)Math.Pow(level, 3) * (100 - level) / 50,
            <= 68 => (int)Math.Pow(level, 3) * (150 - level) / 100,
            <= 98 => (int)Math.Pow(level, 3) * ((1911 - 10 * level) / 3) / 500,
            _ => (int)Math.Pow(level, 3) * (160 - level) / 100,
        };
    }
}
