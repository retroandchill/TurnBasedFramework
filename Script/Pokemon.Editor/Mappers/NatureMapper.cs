using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class NatureMapper
{
    public static UNature ToNature(this NatureInfo natureInfo, UObject? outer = null)
    {
        return natureInfo.ToNatureInitializer(outer);
    }

    public static partial NatureInfo ToNatureInfo(this UNature nature);

    private static partial NatureInitializer ToNatureInitializer(this NatureInfo nature, UObject? outer = null);
}
