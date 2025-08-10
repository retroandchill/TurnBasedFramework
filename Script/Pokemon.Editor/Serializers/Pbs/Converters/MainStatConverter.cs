using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Serializers.Pbs.Converters;

public sealed class MainStatConverter : PbsConverterBase<FGameplayTag>
{
    public override string WriteCsvValue(FGameplayTag value, PbsScalarDescriptor schema, string? sectionName)
    {
        return value.LeafName.ToString();
    }

    public override FGameplayTag GetCsvValue(string input, PbsScalarDescriptor scalarDescriptor, string? sectionName)
    {
        var inputName = new FName(input);
        return UStat.AnyMainCategory.Split(',')
            .Select(x => new FGameplayTag(x))
            .Select(x => x.GetGameplayTagChildren())
            .SelectMany(x => x.GameplayTags.Concat(x.ParentTags))
            .Single(x => x.LeafName == inputName);
    }
}