using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UEnum]
public enum EPokemonGender : byte
{
    Male = 0,
    Female = 1,
    Genderless = 2,
}

[UEnum]
public enum ESpecialGenderRatio : byte
{
    None,
    MaleOnly,
    FemaleOnly,
    GenderlessOnly
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UGenderRatio : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.Core.GenderRatio";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [UMetaData("Categories", TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Ratio")]
    private ESpecialGenderRatio SpecialGenderRatio { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Ratio")]
    [EditCondition("SpecialGenderRatio == ESpecialGenderRatio::None")]
    [UMetaData("EditConditionHides")]
    private byte FemaleChance { get; init; }

    public bool IsSingleGender
    {
        [UFunction(FunctionFlags.BlueprintPure, Category = "Ratio", DisplayName = "Is Single Gender")]
        get => SpecialGenderRatio != ESpecialGenderRatio.None;
    }

    public void Match(Action<byte> onFemaleChance, Action<EPokemonGender> onSingleGender)
    {
        switch (SpecialGenderRatio)
        {
            case ESpecialGenderRatio.None:
                onFemaleChance(FemaleChance);
                break;
            case ESpecialGenderRatio.MaleOnly:
                onSingleGender(EPokemonGender.Male);
                break;
            case ESpecialGenderRatio.FemaleOnly:
                onSingleGender(EPokemonGender.Female);
                break;
            case ESpecialGenderRatio.GenderlessOnly:
                onSingleGender(EPokemonGender.Genderless);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public T Match<T>(Func<byte, T> onFemaleChance, Func<EPokemonGender, T> onSingleGender)
    {
        return SpecialGenderRatio switch
        {
            ESpecialGenderRatio.None => onFemaleChance(FemaleChance),
            ESpecialGenderRatio.MaleOnly => onSingleGender(EPokemonGender.Male),
            ESpecialGenderRatio.FemaleOnly => onSingleGender(EPokemonGender.Female),
            ESpecialGenderRatio.GenderlessOnly => onSingleGender(EPokemonGender.Genderless),
            _ => throw new InvalidOperationException()
        };
    }
}