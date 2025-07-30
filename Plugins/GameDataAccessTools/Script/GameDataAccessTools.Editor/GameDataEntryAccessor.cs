using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace ManagedGameDataAccessToolsEditor;

public interface IGameDataEntryAccessor<out TEntry> where TEntry : UObject, IGameDataEntry
{
    TEntry Entry { get; }
    
    FName Id { get; set; }
    
    int RowIndex { get; set; }
}