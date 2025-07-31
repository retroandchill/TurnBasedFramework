using System.Text.Json;
using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Json;

public sealed class ItemJsonSerializer([ReadOnly] JsonSerializerOptions jsonSerializerOptions) : GameDataEntryJsonSerializerBase<UItem>
{
    public override string SerializeData(IEnumerable<UItem> entries)
    {
        return JsonSerializer.Serialize(entries.Select(x => x.ToItemInfo()), jsonSerializerOptions);
    }

    public override IEnumerable<UItem> DeserializeData(string source, UObject outer)
    {
        return JsonSerializer.Deserialize<ItemInfo[]>(source, jsonSerializerOptions)!
            .Select(x => x.ToItem(outer));
    }
}
