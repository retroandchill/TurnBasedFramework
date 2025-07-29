using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace GameDataAccessTools.Core.DataRetrieval;

public interface IGameDataRepository<T> : IDataRepository<FName, T> where T : UGameDataEntry
{
    TSubclassOf<T> EntryClass { get; }

    IReadOnlyDictionary<FName, T> Entries { get; }

    IReadOnlyList<T> AllEntries { get; }

    int NumEntries { get; }

    T GetEntry(FName key);

    bool TryGetEntry(int index, [NotNullWhen(true)] out T? entry);

    void Refresh();
}