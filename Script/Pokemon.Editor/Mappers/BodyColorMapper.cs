using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class BodyColorMapper
{
    public static UBodyColor ToBodyColor(this BodyColorInfo BodyColorInfo, UObject? outer = null)
    {
        return BodyColorInfo.ToBodyColorInitializer(outer);
    }
    
    public static partial BodyColorInfo ToBodyColorInfo(this UBodyColor BodyColor);
    
    private static partial BodyColorInitializer ToBodyColorInitializer(this BodyColorInfo BodyColor, UObject? outer = null);
}