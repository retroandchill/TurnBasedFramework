using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using Pokemon.Data.Core;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UEnum]
public enum EDamageCategory : byte
{
    Physical,
    Special,
    Status,
}

[UEnum]
public enum EDamageType : byte
{
    NoDamage,
    FixedPower,
    VariablePower,
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UMove : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Moves";
    public const string MetadataCategory = "Pokemon.Metadata.Moves";
    public const string FunctionCodeCategory = "Pokemon.Battle.Moves.FunctionCode";

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

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText Description { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Classification"
    )]
    [Categories(UType.TagCategory)]
    public FGameplayTag Type { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Classification"
    )]
    public EDamageCategory Category { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    public EDamageType DamageType { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    [ClampMin("5")]
    [UIMin("5")]
    [EditCondition($"{nameof(DamageType)} == DamageType::{nameof(EDamageType.FixedPower)}")]
    [EditConditionHides]
    public int Power { get; init; } = 5;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    public bool AlwaysHits { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    [ClampMin("1")]
    [ClampMax("100")]
    [UIMin("1")]
    [UIMax("100")]
    [EditCondition($"!{nameof(AlwaysHits)}")]
    [EditConditionHides]
    public int Accuracy { get; init; } = 100;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Stats")]
    [ClampMin("1")]
    [UIMin("1")]
    public int TotalPP { get; init; } = 1;

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "BattleUsage"
    )]
    public int Priority { get; init; }

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "BattleUsage"
    )]
    [Categories(UTargetType.TagCategory)]
    public FGameplayTag Target { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Effect")]
    [Categories(FunctionCodeCategory)]
    public FGameplayTag FunctionCode { get; init; }

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Effect")]
    public bool GuaranteedEffect { get; init; } = true;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Effect")]
    [ClampMin("1")]
    [ClampMax("100")]
    [UIMin("1")]
    [UIMax("100")]
    [EditCondition($"!{nameof(GuaranteedEffect)}")]
    [EditConditionHides]
    public int EffectChance { get; init; } = 30;

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [Categories(MetadataCategory)]
    public FGameplayTagContainer Tags { get; init; }

    public bool IsPhysical
    {
        get
        {
            if (DamageType == EDamageType.NoDamage)
            {
                return false;
            }

            if (GetDefault<UGameDataSettings>().MoveCategoryPerMove)
            {
                return GameData.Types.GetEntry(Type).IsPhysicalType;
            }

            return Category == EDamageCategory.Physical;
        }
    }

    public bool IsSpecial
    {
        get
        {
            if (DamageType == EDamageType.NoDamage)
            {
                return false;
            }

            if (GetDefault<UGameDataSettings>().MoveCategoryPerMove)
            {
                return GameData.Types.GetEntry(Type).IsSpecialType;
            }

            return Category == EDamageCategory.Special;
        }
    }

    public bool IsDamaging => Category != EDamageCategory.Status;

    public bool IsStatus => Category == EDamageCategory.Status;
}
