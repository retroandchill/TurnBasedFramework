using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IGameDataRepositoryHandle<TEntry> where TEntry : UGameDataEntry
{
    static abstract IGameDataRepository<TEntry> Repository { get; }

    static abstract IEnumerable<FName> EntryNames { get; }

    bool IsValid { get; }
    
    FName Id { get; }

    TEntry? Entry { get; }
}