using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class TypePbsSerializer : PbsSerializerBase<UType>
{
    
    public override string SerializeData(IEnumerable<UType> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToTypeInfo()));
    }

    public override IEnumerable<UType> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<TypeInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToType(outer));
    }
}