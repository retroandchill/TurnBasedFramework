using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class SpeciesPbsSerializer : IGameDataEntrySerializer<USpecies>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<USpecies> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToSpeciesInfo()));
    }

    public IEnumerable<USpecies> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<SpeciesInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToSpecies(outer));
    }
}
