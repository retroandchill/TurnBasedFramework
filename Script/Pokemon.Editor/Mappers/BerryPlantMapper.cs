using Pokemon.Data.Pbs;
using Pokemon.Editor.Model.Data.Pbs;
using Riok.Mapperly.Abstractions;
using UnrealSharp.CoreUObject;

namespace Pokemon.Editor.Mappers;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    PreferParameterlessConstructors = false
)]
public static partial class BerryPlantMapper
{
    public static UBerryPlant ToBerryPlant(
        this BerryPlantInfo berryPlantInfo,
        UObject? outer = null
    )
    {
        return berryPlantInfo.ToBerryPlantInitializer(outer);
    }

    [MapProperty(nameof(berryPlant), nameof(BerryPlantInfo.Yield))]
    public static partial BerryPlantInfo ToBerryPlantInfo(this UBerryPlant berryPlant);

    private static partial BerryPlantInitializer ToBerryPlantInitializer(
        this BerryPlantInfo berryPlant,
        UObject? outer = null
    );

    [MapProperty(nameof(UBerryPlant.MinimumYield), nameof(IntBounds.Min))]
    [MapProperty(nameof(UBerryPlant.MaximumYield), nameof(IntBounds.Max))]
    private static partial IntBounds ToIntBounds(this UBerryPlant berryPlant);

    [MapProperty(nameof(IntBounds.Min), nameof(FInt32Range.LowerBound))]
    [MapProperty(nameof(IntBounds.Max), nameof(FInt32Range.UpperBound))]
    private static partial FInt32Range ToInt32Range(this IntBounds berryPlant);

    private static FInt32RangeBound ToInt32RangeBound(this int bound)
    {
        return new FInt32RangeBound { Value = bound, Type = ERangeBoundTypes.Inclusive };
    }
}
