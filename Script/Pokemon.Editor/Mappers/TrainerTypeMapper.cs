using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class TrainerTypeMapper
{
    public static UTrainerType ToTrainerType(this TrainerTypeInfo typeInfo, UObject? outer = null)
    {
        return typeInfo.ToTrainerTypeInitializer(outer);
    }
    
    [MapPropertyFromSource(nameof(TrainerTypeInfo.SkillLevel), Use = nameof(MapNullableSkillLevel))]
    public static partial TrainerTypeInfo ToTrainerTypeInfo(this UTrainerType type);
    
    
    [MapPropertyFromSource(nameof(TrainerTypeInitializer.SkillLevel), Use = nameof(MapSkillLevel))]
    private static partial TrainerTypeInitializer ToTrainerTypeInitializer(
        this TrainerTypeInfo type,
        UObject? outer = null
    );
    
    private static int MapSkillLevel(this TrainerTypeInfo trainerType)
    {
        return trainerType.SkillLevel ?? trainerType.BasePayout;
    }
    
    private static int? MapNullableSkillLevel(this UTrainerType trainerType)
    {
        return trainerType.SkillLevel != trainerType.BasePayout ? trainerType.SkillLevel : null;
    }
}