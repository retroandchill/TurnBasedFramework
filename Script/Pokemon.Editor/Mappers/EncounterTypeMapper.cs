using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class EncounterTypeMapper
{
    public static UEncounterType ToEncounterType(this EncounterTypeInfo encounterTypeInfo, UObject? outer = null)
    {
        return encounterTypeInfo.ToEncounterTypeInitializer(outer);
    }

    public static partial EncounterTypeInfo ToEncounterTypeInfo(this UEncounterType encounterType);

    private static partial EncounterTypeInitializer ToEncounterTypeInitializer(this EncounterTypeInfo encounterType, UObject? outer = null);
}
