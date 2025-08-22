using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public class MovePbsSerializer : IGameDataEntrySerializer<UMove>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UMove> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToMoveInfo()));
    }

    public IEnumerable<UMove> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<MoveInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToMove(outer));
    }
}
