using Pokemon.Data.Pbs;
using UnrealSharp;
using UnrealSharp.GameplayTags;

namespace Pokemon.Editor.Model.Data.Pbs;

public record ItemInfo
{
    public required FGameplayTag Id { get; init; }
    public required int RowIndex { get; init; }
    public required FText DisplayName { get; init; }
    
    public required FText DisplayNamePlural { get; init; }
    
    public FText? PortionDisplayName { get; init; }
    
    public FText? PortionDisplayNamePlural { get; init; }
    
    public required bool ShouldShowQuantity { get; init; }
    
    public required FText Description { get; init; }
    
    public required FGameplayTag Pocket { get; init; }

    public bool CanSell { get; init; } = true;
    
    public required int Price { get; init; }
    
    public int? SellPrice { get; init; }
    
    public int? BpPrice { get; init; }
    
    public required EFieldUse FieldUse { get; init; }
    
    public required EFieldUse BattleUse { get; init; }

    public FGameplayTagContainer BattleUsageCategories { get; init; } = new()
    {
        GameplayTags = [],
        ParentTags = []
    };
    
    public bool IsConsumable { get; init; }
    
    public FGameplayTag? Move { get; init; }
    
    public FGameplayTagContainer Tags { get; init; }
}
