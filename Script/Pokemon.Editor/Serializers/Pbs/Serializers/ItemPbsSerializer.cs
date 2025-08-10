using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public class ItemPbsSerializer : PbsSerializerBase<UItem>
{
    public override string SerializeData(IEnumerable<UItem> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToItemInfo()));
    }

    public override IEnumerable<UItem> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler.CompilePbsFile<ItemInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToItem(outer));
    }
}