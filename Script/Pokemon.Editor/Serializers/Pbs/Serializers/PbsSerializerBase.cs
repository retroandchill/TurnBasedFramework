using GameDataAccessTools.Core.DataRetrieval;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public abstract class PbsSerializerBase<T> : IGameDataEntrySerializer<T> where T : UObject, IGameDataEntry
{
    public FName FormatTag => "PBS";
    public FText FormatName => "PBS";
    public string FileExtensionText => "PBS file |*.txt|";

    public abstract string SerializeData(IEnumerable<T> entries);

    public abstract IEnumerable<T> DeserializeData(string source, UObject outer);
}