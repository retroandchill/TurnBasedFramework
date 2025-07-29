using System.Diagnostics.CodeAnalysis;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IGameDataRepositoryHandle<TEntry> : IDataHandle<FName, TEntry> where TEntry : UGameDataEntry
{
    new static abstract IGameDataRepository<TEntry> Repository { get; }
}