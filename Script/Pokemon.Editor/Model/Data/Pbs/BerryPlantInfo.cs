using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public readonly record struct IntBounds([PbsRange<int>(0)] int Min, 
                                        [PbsRange<int>(1)] int Max);

public record BerryPlantInfo
{
    [PbsKey]
    [PbsGameplayTag(UBerryPlant.TagCategory)]
    public required FGameplayTag Id { get; init; }
    
    [PbsIndex]
    public int RowIndex { get; init; }
    
    [PbsRange<int>(1)]
    public int HoursPerStage { get; init; } = 3;
    
    [PbsRange<int>(0)]
    public int DryingPerHour { get; init; } = 15;
    
    public IntBounds Yield { get; init; } = new(2, 5);
}
