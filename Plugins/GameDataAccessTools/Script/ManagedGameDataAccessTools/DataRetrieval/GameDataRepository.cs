using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IGameDataRepository<T> where T : UGameDataEntry {

  TSubclassOf<T> EntryClass { get; }

  IReadOnlyDictionary<FName, T> Entries { get; }

  IReadOnlyList<T> OrderedEntries { get; }

  int NumEntries { get; }

  IEnumerable<FName> EntryNames { get; }

  T GetEntry(FName key);

  T GetEntry(int index);

  bool TryGetEntry(FName key, [NotNullWhen(true)] out T? entry);

  bool TryGetEntry(int index, [NotNullWhen(true)] out T? entry);

  void OnGameDataAssetLoaded();
}
