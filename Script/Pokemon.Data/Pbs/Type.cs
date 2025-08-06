using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Pbs;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UType : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Types";
    public const string MetadataCategory = "Pokemon.Metadata.Types";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }

    public bool IsPhysicalType
    {
        [UFunction(FunctionFlags.BlueprintPure, DisplayName = "Is Physical Type", Category = "Type Info")]
        get => !IsSpecialType;
    }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Type Info")]
    public bool IsSpecialType { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Type Info")]
    public bool IsPseudoType  { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Type Info")]
    [Categories(TagCategory)]
    public FGameplayTagContainer Weaknesses { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Type Info")]
    [Categories(TagCategory)]
    public FGameplayTagContainer Resistances { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Type Info")]
    [Categories(TagCategory)]
    public FGameplayTagContainer Immunities { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Metadata")]
    [Categories(MetadataCategory)]
    public FGameplayTagContainer Tags { get; init; }

    [UFunction(FunctionFlags.BlueprintPure, Category = "Types")]
    public float GetEffectiveness([Categories(TagCategory)] FGameplayTag otherType)
    {
        if (!otherType.IsValid) return Effectiveness.NormalMultiplier;
        
        if (Weaknesses.HasTag(otherType)) return Effectiveness.SuperEffectiveMultiplier;
        if (Resistances.HasTag(otherType)) return Effectiveness.NotVeryEffectiveMultiplier;
        
        return Immunities.HasTag(otherType) ? Effectiveness.IneffectiveMultiplier : Effectiveness.NormalMultiplier;
    }
}

public static class Effectiveness
{
    public const float SuperEffectiveMultiplier = 2;
    public const float NotVeryEffectiveMultiplier = 0.5f;
    public const float NormalMultiplier = 1;
    public const float IneffectiveMultiplier = 0;

    private const float Tolerance = 0.0001f;

    public static bool IsIneffective(float value)
    {
        return Math.Abs(value - IneffectiveMultiplier) < Tolerance;
    }

    public static bool IsNotVeryEffective(float value)
    {
        return value is > IneffectiveMultiplier and < NormalMultiplier;
    }

    public static bool IsResistant(float value)
    {
        return value < NormalMultiplier;
    }

    public static bool IsNormal(float value)
    {
        return Math.Abs(value - NormalMultiplier) < Tolerance;
    }

    public static bool IsSuperEffective(float value)
    {
        return value > NormalMultiplier;
    }

    public static bool IsIneffective_type(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var value = Calculate(attackType, defendTypes);
        return IsIneffective(value);
    }

    public static bool IsNotVeryEffectiveType(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var value = Calculate(attackType, defendTypes);
        return IsNotVeryEffective(value);
    }

    public static bool IsResistantType(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var value = Calculate(attackType, defendTypes);
        return IsResistant(value);
    }

    public static bool IsNormalType(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var value = Calculate(attackType, defendTypes);
        return IsNormal(value);
    }
    
    public static bool IsSuperEffectiveType(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var value = Calculate(attackType, defendTypes);
        return IsSuperEffective(value);
    }

    public static float GetTypeEffectiveness(this FGameplayTag attackType, FGameplayTag defendType)
    {
        return GameData.Types.GetEntry(attackType).GetEffectiveness(defendType);
    }

    public static float Calculate(this FGameplayTag attackType, params ReadOnlySpan<FGameplayTag> defendTypes)
    {
        var ret = NormalMultiplier;
        foreach (var defendType in defendTypes)
        {
            ret *= attackType.GetTypeEffectiveness(defendType) / NormalMultiplier;
        }

        return ret;
    }
}