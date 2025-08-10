using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UBerryPlant : UObject, IGameDataEntry
{
    public const string TagCategory = UItem.TagCategory;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Growth")]
    [ClampMin("1")]
    [UIMin("1")]
    public int HoursPerStage { get; init; } = 3;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Growth")]
    [ClampMin("1")]
    [UIMin("1")]
    public int DryingPerHour { get; init; } = 15;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Growth")]
    [ClampMin("1")]
    [UIMin("1")]
    private FInt32Range Yield { get; init; }

    public UBerryPlant()
    {
        Yield = new FInt32Range
        {
            LowerBound = new FInt32RangeBound
            {
                Type = ERangeBoundTypes.Inclusive,
                Value = 2
            },
            UpperBound = new FInt32RangeBound
            {
                Type = ERangeBoundTypes.Inclusive,
                Value = 5
            }
        };
    }

    public int MinimumYield
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Growth")]
        get => Yield.LowerBound.Value;
    }
    
    public int MaximumYield
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Growth")]
        get => Yield.UpperBound.Value;
    }
}