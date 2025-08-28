using Pokemon.Core.Characters;
using Pokemon.Core.Executor.Exp;
using UnrealSharp.Attributes;
using UnrealSharp.Core;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.GameplayTags;

namespace Pokemon.Core;

public static class PokemonStatics
{
    internal static UObject WorldContextObject
    {
        get
        {
            var worldContextObject = FCSManagerExporter.CallGetCurrentWorldContext();
            var worldContextHandle = FCSManagerExporter.CallFindManagedObject(worldContextObject);
            return GCHandleUtilities.GetObjectFromHandlePtr<UObject>(worldContextHandle)!;
        }
    }
    
    public static UTrainer Player => WorldContextObject.GetGameInstanceSubsystem<UPokemonSubsystem>().Player;
    
    public static IExpGrowthFormula GetExpGrowthFormula(FGameplayTag growthRate)
    {
        return WorldContextObject.GetGameInstanceSubsystem<UPokemonSubsystem>().GetExpGrowthFormula(growthRate);
    }
}

[UClass]
public class UPokemonStatic : UBlueprintFunctionLibrary
{
    [UFunction(FunctionFlags.BlueprintPure, Category = "Pokémon")]
    public static UTrainer GetPlayer()
    {
        return PokemonStatics.Player;
    }
}