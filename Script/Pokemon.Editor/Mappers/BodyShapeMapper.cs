using Pokemon.Data.Core;
using Pokemon.Editor.Model.Data.Core;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessToolsEditor;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class BodyShapeMapper
{
    public static UBodyShape ToBodyShape(this BodyShapeInfo bodyShapeInfo, UObject? outer = null)
    {
        return bodyShapeInfo.ToBodyShapeInitializer(outer);
    }

    public static partial BodyShapeInfo ToBodyShapeInfo(this UBodyShape bodyShape);

    private static partial BodyShapeInitializer ToBodyShapeInitializer(
        this BodyShapeInfo bodyShape,
        UObject? outer = null
    );
}
