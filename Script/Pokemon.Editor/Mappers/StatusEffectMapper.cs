using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class StatusEffectMapper
{
    public static UStatusEffect ToStatusEffect(this StatusEffectInfo statusEffectInfo, UObject? outer = null)
    {
        return statusEffectInfo.ToStatusEffectInitializer(outer);
    }

    public static partial StatusEffectInfo ToStatusEffectInfo(this UStatusEffect statusEffect);

    private static partial StatusEffectInitializer ToStatusEffectInitializer(this StatusEffectInfo statusEffect, UObject? outer = null);
}
