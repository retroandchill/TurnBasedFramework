using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public class ItemPbsSerializer : IGameDataEntrySerializer<UItem>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UItem> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToItemInfo()));
    }

    public IEnumerable<UItem> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<ItemInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToItem(outer));
    }
}
