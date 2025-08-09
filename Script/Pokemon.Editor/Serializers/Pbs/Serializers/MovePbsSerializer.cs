using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public class MovePbsSerializer : PbsSerializerBase<UMove>
{
    public override string SerializeData(IEnumerable<UMove> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToMoveInfo()));
    }

    public override IEnumerable<UMove> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<MoveInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToMove(outer));
    }
}