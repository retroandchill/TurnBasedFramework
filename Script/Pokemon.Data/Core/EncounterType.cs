using GameAccessTools.SourceGenerator.Attributes;
using GameDataAccessTools.Core.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Attributes;
using UnrealSharp.Attributes.MetaTags;
using UnrealSharp.CoreUObject;
using UnrealSharp.GameDataAccessTools;
using UnrealSharp.GameplayTags;

namespace Pokemon.Data.Core;

[UClass(ClassFlags.EditInlineNew)]
[GameDataEntry]
public class UEncounterType : UObject, IGameDataEntry
{
    public const string TagCategory = "Pokemon.Data.EncounterTypes";
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Identification")]
    [Categories(TagCategory)]
    public FGameplayTag Id { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.VisibleAnywhere, Category = "Identification")]
    public int RowIndex { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Display")]
    public FText DisplayName { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Encounters")]
    [Categories("Pokemon.Field.Encounters.Triggers")]
    public FGameplayTag Trigger { get; init; }
    
    [UProperty(PropertyFlags.BlueprintReadOnly | PropertyFlags.EditAnywhere, Category = "Encounters")]
    public int TriggerChance { get; init; }
}