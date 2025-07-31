using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class TypeMapper
{
    public static UType ToType(this TypeInfo typeInfo, UObject? outer = null)
    {
        return typeInfo.ToTypeInitializer(outer);
    }

    public static partial TypeInfo ToTypeInfo(this UType type);

    private static partial TypeInitializer ToTypeInitializer(this TypeInfo type, UObject? outer = null);
}
