using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class StatMapper
{
    public static UStat ToStat(this StatInfo statInfo, UObject? outer = null)
    {
        return statInfo.ToStatInitializer(outer);
    }

    public static partial StatInfo ToStatInfo(this UStat stat);

    private static partial StatInitializer ToStatInitializer(this StatInfo stat, UObject? outer = null);
}
