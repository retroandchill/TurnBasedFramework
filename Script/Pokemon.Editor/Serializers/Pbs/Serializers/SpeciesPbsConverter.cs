using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class SpeciesPbsSerializer : PbsSerializerBase<USpecies>
{
    
    public override string SerializeData(IEnumerable<USpecies> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToSpeciesInfo()));
    }

    public override IEnumerable<USpecies> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<SpeciesInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToSpecies(outer));
    }
}