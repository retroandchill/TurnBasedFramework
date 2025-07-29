using System.Diagnostics.CodeAnalysis;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IDataRepository<TKey, TData> where TKey : notnull
{
    IEnumerable<TData> AllEntries { get; }
    
    IEnumerable<TKey> EntryKeys { get; }
    
    TData GetEntry(TKey key);
    
    bool TryGetEntry(TKey key, [NotNullWhen(true)] out TData? entry);
}