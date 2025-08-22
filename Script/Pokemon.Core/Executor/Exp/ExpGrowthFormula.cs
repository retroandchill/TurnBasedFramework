using Pokemon.Data;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public interface IExpGrowthFormula
{
    static int MaxLevel => UObject.GetDefault<UGameDataSettings>().MaxLevel;

    FGameplayTag GrowthRateFor { get; }

    int GetMinimumExpForLevel(int level);

    sealed int MaximumExp => GetMinimumExpForLevel(MaxLevel);

    sealed int AddExp(int exp1, int exp2) => Math.Clamp(exp1 + exp2, 0, MaximumExp);

    sealed int GetLevelForExp(int exp)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(exp, 0);
        var max = MaxLevel;
        if (exp >= MaximumExp)
            return max;

        for (var i = 0; i <= max; i++)
        {
            if (exp < GetMinimumExpForLevel(i))
                return i - 1;
        }

        return max;
    }
}
