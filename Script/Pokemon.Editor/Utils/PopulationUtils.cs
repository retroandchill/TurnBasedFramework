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
        var evolutionMethodRepo = GetEvolutionMethodRepo();
        targetMap.Clear();
        foreach (var entry in evolutionMethodRepo.AllEntries)
        {
            targetMap.Add(entry.DisplayName.ToString(), entry.Id);
        }
    }

    public static TSubclassOf<UEvolutionConditionData> GetEvolutionMethodClass(FGameplayTag tag)
    {
        var evolutionMethodRepo = GetEvolutionMethodRepo();
        return evolutionMethodRepo.TryGetEntry(tag, out var entry) ? entry.ConditionType : default;
    }

    private static IGameDataRepository<UEvolutionMethod> GetEvolutionMethodRepo()
    {
        var settings = UObject.GetDefault<UGameDataSettings>();
        var evolutionMethodRepo = ((TSoftObjectPtr<UObject>)settings.EvolutionMethods).LoadSynchronous() as IGameDataRepository<UEvolutionMethod>;
        ArgumentNullException.ThrowIfNull(evolutionMethodRepo);
        evolutionMethodRepo.Refresh();
        return evolutionMethodRepo;
    }
}