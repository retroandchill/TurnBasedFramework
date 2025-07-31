using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace GameDataAccessTools.Core.DataRetrieval;

public interface IGameDataRepository<T> : IDataRepository<FGameplayTag, T> where T : UObject, IGameDataEntry
{
    TSubclassOf<T> EntryClass { get; }

    IReadOnlyDictionary<FGameplayTag, T> Entries { get; }

    int NumEntries { get; }

    T GetEntry(int key);

    bool TryGetEntry(int index, [NotNullWhen(true)] out T? entry);

    void Refresh();
}