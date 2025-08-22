using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class FluctuatingExpGrowthFormula : IExpGrowthFormula
{
    public FGameplayTag GrowthRateFor => GameplayTags.Pokemon_Data_GrowthRates_Fluctuating;

    public int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level switch
        {
            1 => 0,
            <= 15 => (int)Math.Pow(level, 3) * (24 + (level + 1) / 3) / 50,
            <= 35 => (int)Math.Pow(level, 3) * (14 + level) / 50,
            _ => (int)Math.Pow(level, 3) * (32 + level / 2) / 50,
        };
    }
}
