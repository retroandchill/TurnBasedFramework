using UnrealSharp.GameplayTags;

namespace GameDataAccessTools.Core.DataRetrieval;

public interface IGameDataEntry
{
    FGameplayTag Id { get; }
    
    int RowIndex { get; }
}