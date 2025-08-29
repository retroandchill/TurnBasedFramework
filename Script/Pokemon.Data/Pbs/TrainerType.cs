using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UEnum]
public enum ETrainerGender : byte
{
    /// <summary>
    /// Represents a male trainer,
    /// </summary>
    Male,

    /// <summary>
    /// Represents a female trainer
    /// </summary>
    Female,

    /// <summary>
    /// Represents a trainer of unknown gender
    /// </summary>
    Unknown,

    /// <summary>
    /// Represents a double battle trainer with a male and female member
    /// </summary>
    Mixed,
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UTrainerType : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.TrainerTypes";
    public const string MetadataCategory = "Pokemon.Metadata.TrainerTypes";
    
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Identification"
    )]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere,
        Category = "Identification"
    )]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Profile")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Profile")]
    public ETrainerGender Gender { get; init; } = ETrainerGender.Unknown;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    [UIMin("0")]
    [ClampMin("0")]
    public int BasePayout { get; init; } = 30;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Battle")]
    [UIMin("0")]
    [ClampMin("0")]
    public int SkillLevel { get; init; } = 30;
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [Categories(MetadataCategory)]
    public FGameplayTagContainer Tags { get; init; }
}
