using System.Diagnostics.CodeAnalysis;
using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using Pokemon.Data.Attributes;
using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UClass(ClassFlags.EditInlineNew | ClassFlags.Abstract)]
public class UEvolutionConditionData : UObject;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEvolutionMethod : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.EvolutionMethods";

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

    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        Category = "Evolution"
    )]
    public TSubclassOf<UEvolutionConditionData> ConditionType { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public sealed class UIntEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    public int Parameter { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public sealed class UMoveEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        DisplayName = "Move ID"
    )]
    [Categories(UMove.TagCategory)]
    [ParentTag(UMove.TagCategory)]
    public FGameplayTag MoveId { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public class UTypeEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        DisplayName = "Type ID"
    )]
    [Categories(UType.MetadataCategory)]
    [ParentTag(UType.TagCategory)]
    public FGameplayTag TypeId { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public class UItemEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        DisplayName = "Item ID"
    )]
    [Categories(UItem.TagCategory)]
    [ParentTag(UItem.TagCategory)]
    public FGameplayTag ItemId { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public class USpeciesEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(
        PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere,
        DisplayName = "Species ID"
    )]
    [Categories(USpecies.TagCategory)]
    [ParentTag(USpecies.TagCategory)]
    public FGameplayTag SpeciesId { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public class ULocationEvolutionConditionData : UEvolutionConditionData
{
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    public TSoftObjectPtr<UWorld> Level { get; init; }
}

[UClass(ClassFlags.EditInlineNew)]
public class ULocationFlagEvolutionConditionData : UEvolutionConditionData
{
    public const string TagCategory = "Pokemon.Field.Location.Evolution";

    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere)]
    [Categories(TagCategory)]
    [ParentTag(TagCategory)]
    public FGameplayTag Flag { get; init; }
}
