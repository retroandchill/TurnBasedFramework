using UnrealSharp;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor;

public interface IGameDataEntryAccessor<out TEntry> where TEntry : UGameDataEntry
{
    TEntry Entry { get; }
    
    FName Id { get; set; }
    
    int RowIndex { get; set; }
}