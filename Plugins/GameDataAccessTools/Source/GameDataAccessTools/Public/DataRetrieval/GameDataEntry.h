// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "GameDataEntry.generated.h"

/**
 *
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UGameDataEntry : public UObject
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    FName GetId() const
    {
        return Id;
    }

    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    int32 GetRowIndex() const
    {
        return RowIndex;
    }

private:
#if WITH_EDITORONLY_DATA
    friend class FGameDataRepositoryEditor;
#endif

    UPROPERTY(EditAnywhere, BlueprintGetter=GetId, Category = "EntryInformation", meta = (JsonOrder = "0"))
    FName Id;

    UPROPERTY(VisibleAnywhere, BlueprintGetter=GetRowIndex, Category = "EntryInformation", meta = (JsonOrder = "1"))
    int32 RowIndex;
};
