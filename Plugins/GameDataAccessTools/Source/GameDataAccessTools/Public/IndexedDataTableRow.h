// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"

#include "IndexedDataTableRow.generated.h"

/**
 * 
 */
USTRUCT(BlueprintInternalUseOnly)
struct GAMEDATAACCESSTOOLS_API FIndexedTableRow : public FTableRowBase {
    GENERATED_BODY()

    /**
     * The internal ID used for lookup by the game
     */
    UPROPERTY(BlueprintReadOnly, VisibleAnywhere, Category = "Metadata")
    FName ID;

    /**
     * The number of this row in the data table
     */
    UPROPERTY(BlueprintReadOnly, VisibleAnywhere, Category = "Metadata")
    int32 RowID;

    void OnDataTableChanged(const UDataTable *InDataTable, const FName InRowName) override;
};
