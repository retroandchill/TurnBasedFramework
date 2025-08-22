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
public static partial class EnvironmentMapper
{
    public static UEnvironment ToEnvironment(
        this EnvironmentInfo environmentInfo,
        UObject? outer = null
    )
    {
        return environmentInfo.ToEnvironmentInitializer(outer);
    }

    public static partial EnvironmentInfo ToEnvironmentInfo(this UEnvironment environment);

    private static partial EnvironmentInitializer ToEnvironmentInitializer(
        this EnvironmentInfo environment,
        UObject? outer = null
    );
}
