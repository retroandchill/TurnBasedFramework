using ManagedGameDataAccessToolsEditor.Serialization;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor.Interop;

public class GameDataEntryJsonSerializer<TEntry> : IGameDataEntrySerializer<TEntry> where TEntry : UGameDataEntry
{
    public FText FormatName => "JSON";
    
    public Task SerializeData(IEnumerable<TEntry> entries, string destination, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TEntry> DeserializeData(string source, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}