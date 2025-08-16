// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ManagedTestCase.generated.h"

/**
 * 
 */
USTRUCT(BlueprintType)
struct FManagedTestCase
{
    GENERATED_BODY()
    
    UPROPERTY(BlueprintReadOnly)
    FString FullyQualifiedName;

    UPROPERTY(BlueprintReadOnly)
    FString ExecutorUri;

    UPROPERTY(BlueprintReadOnly)
    FString Source;

    UPROPERTY(BlueprintReadOnly)
    FString CodeFilePath;

    UPROPERTY(BlueprintReadOnly)
    int32 LineNumber;
};
