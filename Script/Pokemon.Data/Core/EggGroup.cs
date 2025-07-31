using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UEnum]
public enum EEggGroupType : byte
{
    WithSameEggGroup,
    WithAnyEggGroup,
    WithNoEggGroups
}

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEggGroup : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.EggGroups";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [UMetaData("Categories", TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Breeding")]
    public EEggGroupType BreedingType { get; init; }
}