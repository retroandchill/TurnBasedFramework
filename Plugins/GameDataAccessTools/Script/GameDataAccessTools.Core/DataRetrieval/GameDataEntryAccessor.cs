using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace GameDataAccessTools.Core.DataRetrieval;

public interface IGameDataEntryInitializer<out TEntry> where TEntry : UObject, IGameDataEntry
{
    TEntry Entry { get; }
    
    FGameplayTag Id { get; init; }
    
    int RowIndex { get; init; }
}