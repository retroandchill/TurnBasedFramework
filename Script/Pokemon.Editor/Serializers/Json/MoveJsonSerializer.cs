using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class MoveJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UMove>
{
    public override string SerializeData(IEnumerable<UMove> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToMoveInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UMove> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<MoveInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToMove(outer));
    }
}
