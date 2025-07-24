using UnrealSharp;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor.Serialization;

public interface IGameDataEntrySerializer
{
    FText FormatName { get; }
}

public interface IGameDataEntrySerializer<TEntry> : IGameDataEntrySerializer where TEntry : UGameDataEntry
{
    Task SerializeData(IEnumerable<TEntry> entries, string destination, CancellationToken cancellationToken = default);
    
    IAsyncEnumerable<TEntry> DeserializeData(string source, CancellationToken cancellationToken = default);
}