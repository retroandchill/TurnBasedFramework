using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class HabitatMapper
{
    public static UHabitat ToHabitat(this HabitatInfo habitatInfo, UObject? outer = null)
    {
        return habitatInfo.ToHabitatInitializer(outer);
    }

    public static partial HabitatInfo ToHabitatInfo(this UHabitat habitat);

    private static partial HabitatInitializer ToHabitatInitializer(this HabitatInfo habitat, UObject? outer = null);
}
