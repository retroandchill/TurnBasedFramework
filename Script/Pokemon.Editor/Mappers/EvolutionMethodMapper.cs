using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class EvolutionMethodMapper
{
    public static UEvolutionMethod ToEvolutionMethod(this EvolutionMethodInfo evolutionMethodInfo, UObject? outer = null)
    {
        return evolutionMethodInfo.ToEvolutionMethodInitializer(outer);
    }

    public static partial EvolutionMethodInfo ToEvolutionMethodInfo(this UEvolutionMethod evolutionMethod);

    private static partial EvolutionMethodInitializer ToEvolutionMethodInitializer(this EvolutionMethodInfo evolutionMethod, UObject? outer = null);
}
