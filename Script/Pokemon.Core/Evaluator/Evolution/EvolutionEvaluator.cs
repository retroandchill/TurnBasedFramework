using Pokemon.Core.Player.Pokemon;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Core.Evaluator.Evolution;

[UClass(ClassFlags.EditInlineNew | ClassFlags.Abstract)]
public class UEvolutionEvaluator : UObject
{
    public virtual FGameplayTag Trigger
    {
        [UFunction(FunctionFlags.BlueprintEvent, Category = "Evolution")]
        get => throw new NotImplementedException();
    }
    
    [UFunction(FunctionFlags.BlueprintEvent, Category = "Evolution")]
    public virtual bool ShouldEvolve(UPokemon pokemon) => throw new NotImplementedException();
}