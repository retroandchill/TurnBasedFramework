using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;

namespace GameDataAccessTools.Core.Serialization;

public interface IGameDataEntrySerializer
{
    FName FormatTag { get; }

    FText FormatName { get; }

    string FileExtensionText { get; }
}

public interface IGameDataEntrySerializer<TEntry> : IGameDataEntrySerializer
    where TEntry : UObject, IGameDataEntry
{
    string SerializeData(IEnumerable<TEntry> entries);

    IEnumerable<TEntry> DeserializeData(string source, UObject outer);
}
