using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class BerryPlantPbsSerializer : PbsSerializerBase<UBerryPlant>
{
    public override string SerializeData(IEnumerable<UBerryPlant> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToBerryPlantInfo()));
    }

    public override IEnumerable<UBerryPlant> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<BerryPlantInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToBerryPlant(outer));
    }
}