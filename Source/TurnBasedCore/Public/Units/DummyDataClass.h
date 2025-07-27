// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "DummyDataClass.generated.h"

USTRUCT(BlueprintType)
struct FDummyDataStruct
{
    GENERATED_BODY()

public:
    UPROPERTY(EditAnywhere, meta = (GetOptions = "DummyDataClass.GetOptions"))
    FName Option;
};

/**
 * 
 */
UCLASS()
class TURNBASEDCORE_API UDummyDataClass : public UObject
{
    GENERATED_BODY()

public:
    UFUNCTION()
    static TArray<FName> GetOptions();
};
