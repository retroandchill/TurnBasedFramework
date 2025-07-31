using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class GenderRatioMapper
{
    public static UGenderRatio ToGenderRatio(this GenderRatioInfo genderRatioInfo, UObject? outer = null)
    {
        return genderRatioInfo.ToGenderRatioInitializer(outer);
    }

    public static GenderRatioInfo ToGenderRatioInfo(this UGenderRatio genderRatio)
    {
        return GenderRatioInitializer.From(genderRatio).ToGenderRatioInfo();
    }

    private static partial GenderRatioInfo ToGenderRatioInfo(this GenderRatioInitializer genderRatio);

    [MapProperty(nameof(genderRatio), nameof(GenderRatioInitializer.FemaleChance))]
    private static partial GenderRatioInitializer ToGenderRatioInitializer(
        this GenderRatioInfo genderRatio, UObject? outer = null);

    private static byte GetFemaleChance(this GenderRatioInfo genderRatio)
    {
        return genderRatio.FemaleChance ?? 0;
    }
}