using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class TrainerTypePbsSerializer : IGameDataEntrySerializer<UTrainerType>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UTrainerType> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToTrainerTypeInfo()));
    }

    public IEnumerable<UTrainerType> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<TrainerTypeInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToTrainerType(outer));
    }
}
