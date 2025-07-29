using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;

namespace Pokemon.Core.Evaluator.GrowthRates;

[UClass(ClassFlags.Abstract)]
public class UGrowthRateFormula : UObject
{
    [UFunction(FunctionFlags.BlueprintEvent)] 
    public virtual int GetMinimumExpForLevel(int level) 
    {
        throw new NotImplementedException();
    }
}