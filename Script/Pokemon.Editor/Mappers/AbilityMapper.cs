using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class AbilityMapper
{
    public static UAbility ToAbility(this AbilityInfo abilityInfo, UObject? outer = null)
    {
        return abilityInfo.ToAbilityInitializer(outer);
    }

    public static partial AbilityInfo ToAbilityInfo(this UAbility ability);

    private static partial AbilityInitializer ToAbilityInitializer(
        this AbilityInfo ability,
        UObject? outer = null
    );
}
