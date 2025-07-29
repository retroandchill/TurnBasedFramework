namespace GameDataAccessTools.Core.DataRetrieval;

public interface IDataHandle<TKey, TData> where TKey : notnull
{
    static abstract IDataRepository<TKey, TData> Repository { get; }
    
    static abstract IEnumerable<TKey> EntryKeys { get; }

    bool IsValid { get; }
    
    TKey Id { get; }

    TData? Entry { get; }
}