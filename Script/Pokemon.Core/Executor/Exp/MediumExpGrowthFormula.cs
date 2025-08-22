using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class MediumExpGrowthFormula : IExpGrowthFormula
{
    public FGameplayTag GrowthRateFor => GameplayTags.Pokemon_Data_GrowthRates_Medium;

    public int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3);
    }
}
