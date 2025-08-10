using GameDataAccessTools.Core.DataRetrieval;
using Pokemon.Data;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Utils;

public static class PopulationUtils
{
    public static void PopulateWithEvolutionData(this IDictionary<FName, FGameplayTag> targetMap)
    {
        var settings = UObject.GetDefault<UGameDataSettings>();
        var evolutionMethodRepo = ((TSoftObjectPtr<UObject>)settings.EvolutionMethods).LoadSynchronous() as IGameDataRepository<UEvolutionMethod>;
        ArgumentNullException.ThrowIfNull(evolutionMethodRepo);
        
        targetMap.Clear();
        foreach (var entry in evolutionMethodRepo.AllEntries)
        {
            targetMap.Add(entry.DisplayName.ToString(), entry.Id);
        }
    }
}