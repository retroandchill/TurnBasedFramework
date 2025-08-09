using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class AbilityPbsSerializer : PbsSerializerBase<UAbility>
{
    public override string SerializeData(IEnumerable<UAbility> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToAbilityInfo()));
    }

    public override IEnumerable<UAbility> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<AbilityInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToAbility(outer));
    }
}