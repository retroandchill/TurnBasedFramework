using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class FastExpGrowthFormula : IExpGrowthFormula
{
    public FGameplayTag GrowthRateFor => GameplayTags.Pokemon_Data_GrowthRates_Fast;

    public int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 4 / 5;
    }
}
