using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor.Serialization;

public interface IGameDataEntrySerializer
{
    FText FormatName { get; }
    
    string FileExtensionText { get; }
}

public interface IGameDataEntrySerializer<TEntry> : IGameDataEntrySerializer where TEntry : UGameDataEntry
{
    string SerializeData(IEnumerable<TEntry> entries);
    
    IEnumerable<TEntry> DeserializeData(string source, UObject outer);
}