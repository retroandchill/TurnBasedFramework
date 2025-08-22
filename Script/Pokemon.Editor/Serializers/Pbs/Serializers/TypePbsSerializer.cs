using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class TypePbsSerializer : IGameDataEntrySerializer<UType>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UType> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToTypeInfo()));
    }

    public IEnumerable<UType> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<TypeInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToType(outer));
    }
}
