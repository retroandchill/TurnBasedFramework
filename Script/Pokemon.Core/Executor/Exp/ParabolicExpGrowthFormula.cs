using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class ParabolicExpGrowthFormula : IExpGrowthFormula
{
    public FGameplayTag GrowthRateFor => GameplayTags.Pokemon_Data_GrowthRates_Parabolic;
    public int GetMinimumExpForLevel(int level)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(level, 0);
        return level == 1 ? 0 : (int)Math.Pow(level, 3) * 6 / 5 - 15 * (int)Math.Pow(level, 2) + 100 * level - 140;
    }
}