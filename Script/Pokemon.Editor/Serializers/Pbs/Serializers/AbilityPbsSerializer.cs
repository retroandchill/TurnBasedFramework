using GameDataAccessTools.Core.Serialization;
using Pokemon.Data.Pbs;
using Pokemon.Editor.Mappers;
using Pokemon.Editor.Model.Data.Pbs;
using UnrealSharp;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Serializers.Pbs.Serializers;

public sealed class AbilityPbsSerializer : IGameDataEntrySerializer<UAbility>
{
    public FName FormatTag => PbsConstants.FormatTag;
    public FText FormatName => PbsConstants.FormatName;
    public string FileExtensionText => PbsConstants.FileExtensionText;

    public string SerializeData(IEnumerable<UAbility> entries)
    {
        return PbsCompiler.WritePbs(entries.Select(x => x.ToAbilityInfo()));
    }

    public IEnumerable<UAbility> DeserializeData(string source, UObject outer)
    {
        return PbsCompiler
            .CompilePbsFile<AbilityInfo>(source)
            .Select(x => x.Value)
            .Select(x => x.ToAbility(outer));
    }
}
