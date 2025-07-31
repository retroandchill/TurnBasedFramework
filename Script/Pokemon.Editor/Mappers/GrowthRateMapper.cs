using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class GrowthRateMapper
{
    public static UGrowthRate ToGrowthRate(this GrowthRateInfo growthRateInfo, UObject? outer = null)
    {
        return growthRateInfo.ToGrowthRateInitializer(outer);
    }
    
    public static partial GrowthRateInfo ToGrowthRateInfo(this UGrowthRate growthRate);
    
    private static partial GrowthRateInitializer ToGrowthRateInitializer(this GrowthRateInfo growthRate, UObject? outer = null);
}