using Pokemon.Data.Pbs;
using Pokemon.Editor.Serializers.Pbs.Attributes;
using Pokemon.Editor.Serializers.Pbs.Converters;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record ItemInfo
{
    [PbsKey]
    [PbsGameplayTag(UItem.TagCategory, Create = true)]
    public required FGameplayTag Id { get; init; }
    
    [PbsIndex]
    public int RowIndex { get; init; }
    
    [PbsName("Name")]
    public FText DisplayName { get; init; } = "Unnamed";
    
    [PbsName("NamePlural")]
    public FText DisplayNamePlural { get; init; } = "Unnamed";
    
    [PbsName("PortionName")]
    public FText? PortionDisplayName { get; init; }
    
    [PbsName("PortionNamePlural")]
    public FText? PortionDisplayNamePlural { get; init; }
    
    [PbsName("ShowQuantity")]
    public bool? ShouldShowQuantity { get; init; }

    public FText Description { get; init; } = "???";
    
    [PbsScalar<ItemPocketConverter>]
    public FGameplayTag Pocket { get; init; }
    
    [PbsRange<int>(0)]
    public int Price { get; init; }
    
    [PbsRange<int>(0)]
    public int? SellPrice { get; init; }

    [PbsRange<int>(1)]
    public int BpPrice { get; init; } = 1;
    
    public EFieldUse FieldUse { get; init; }
    
    public EBattleUse BattleUse { get; init; }

    [PbsGameplayTag(UItem.BattleUsageCategory, Create = true)]
    public FGameplayTagContainer BattleUsageCategories { get; init; } = new()
    {
        GameplayTags = [],
        ParentTags = []
    };
    
    [PbsName("Consumable")]
    public bool? IsConsumable { get; init; }
    
    [PbsGameplayTag(UMove.TagCategory)]
    public FGameplayTag? Move { get; init; }
    
    [PbsGameplayTag(UItem.MetadataCategory, Create = true, Separator = "_")]
    public FGameplayTagContainer Tags { get; init; }
}
