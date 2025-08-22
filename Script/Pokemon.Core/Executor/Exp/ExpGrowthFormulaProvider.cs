using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Executor.Exp;

public class ExpGrowthFormulaProvider
{
    private readonly Dictionary<FGameplayTag, IExpGrowthFormula> _expGrowthFormulas;

    public ExpGrowthFormulaProvider(IEnumerable<IExpGrowthFormula> expGrowthFormulas)
    {
        _expGrowthFormulas = expGrowthFormulas.ToDictionary(x => x.GrowthRateFor);
    }

    public IExpGrowthFormula GetGrowthFormula(FGameplayTag growthRate)
    {
        return _expGrowthFormulas[growthRate];
    }
}
