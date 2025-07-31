using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class EggGroupMapper
{
    public static UEggGroup ToEggGroup(this EggGroupInfo eggGroupInfo, UObject? outer = null)
    {
        return eggGroupInfo.ToEggGroupInitializer(outer);
    }

    public static partial EggGroupInfo ToEggGroupInfo(this UEggGroup eggGroup);

    private static partial EggGroupInitializer ToEggGroupInitializer(this EggGroupInfo eggGroup, UObject? outer = null);
}
