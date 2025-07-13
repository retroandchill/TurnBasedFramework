// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "IndexedDataTableRow.h"
#include "UObject/Object.h"
#include "Item.generated.h"

/**
 * Represents the data for an Item
 */
USTRUCT(BlueprintType, meta = (DatabaseType = "PBS"))
struct GAMEDATAACCESSTOOLS_API FItem : public FIndexedTableRow {
    GENERATED_BODY()

    /**
     * Name of this item as displayed by the game.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Basic")
    FText RealName;

    /**
     * Plural name of this item as displayed by the game.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Basic")
    FText RealNamePlural;

    /**
     * Name of a portion of this item as displayed by the game.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Basic")
    FText RealPortionName;

    /**
     * Name of 2 or more portions of this item as displayed by the game.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Basic")
    FText RealPortionNamePlural;

    /**
     * Pocket in the Bag where this item is stored.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "BagInfo",
              meta = (GetOptions = "PokemonData.ItemHelper.GetPocketNames"))
    FName Pocket;

    /**
     * Purchase price of this item.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Marts", meta = (ClampMin = 0, UIMin = 0))
    int32 Price;

    /**
     * Sell price of this item. If blank, is half the purchase price.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Marts", meta = (ClampMin = 0, UIMin = 0))
    int32 SellPrice;

    /**
     * Purchase price of this item in Battle Points (BP).
     */
    UPROPERTY(DisplayName = "BP Price", BlueprintReadOnly, Category = "Marts", EditAnywhere,
              meta = (ClampMin = 1, UIMin = 1))
    int32 BPPrice;

    /**
     * How this item can be used outside of battle.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Usage)
    uint8 FieldUse;

    /**
     * How this item can be used within a battle.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Usage)
    uint8 BattleUse;

    /**
     * Defines the categories that the items can belong to
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Usage, meta = (Categories = "Battle.Items.BattleUse"))
    FGameplayTagContainer BattleUsageCategories;

    /**
     * Words/phrases that can be used to group certain kinds of items."
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Metadata)
    TArray<FName> Tags;

    /**
     * Whether this item is consumed after use.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Usage)
    bool Consumable;

    /**
     * Whether the Bag shows how many of this item are in there.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Display)
    bool ShowQuantity;

    /**
     * Move taught by this HM, TM or TR.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = Usage,
              meta = (GetOptions = "PokemonData.MoveHelper.GetMoveNames"))
    FName Move;

    /**
     * Description of this item.
     */
    UPROPERTY(BlueprintReadOnly, EditAnywhere, Category = "Basic")
    FText Description;
};