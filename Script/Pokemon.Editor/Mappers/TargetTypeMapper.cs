using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class TargetTypeMapper
{
    public static UTargetType ToTargetType(
        this TargetTypeInfo targetTypeInfo,
        UObject? outer = null
    )
    {
        return targetTypeInfo.ToTargetTypeInitializer(outer);
    }

    public static partial TargetTypeInfo ToTargetTypeInfo(this UTargetType targetType);

    private static partial TargetTypeInitializer ToTargetTypeInitializer(
        this TargetTypeInfo targetType,
        UObject? outer = null
    );
}
