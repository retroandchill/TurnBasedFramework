using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class BerryPlantPbsSerializer : IGameDataEntrySerializer<UBerryPlant>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UBerryPlant> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToBerryPlantInfo()));
    }

    public IEnumerable<UBerryPlant> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<BerryPlantInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToBerryPlant(outer));
    }
}
