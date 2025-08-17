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
    FName AssemblyName;
    
    UPROPERTY(BlueprintReadOnly)
    FString FullyQualifiedName;

    UPROPERTY(BlueprintReadOnly)
    FString CodeFilePath;

    UPROPERTY(BlueprintReadOnly)
    int32 LineNumber;
};

struct FManagedTestCaseHandle
{
    const FManagedTestCase TestCase;
    const FSharedGCHandle ManagedTestCase;

    FManagedTestCaseHandle(FManagedTestCase InTestCase, const FGCHandleIntPtr InManagedTestCase)
        : TestCase(MoveTemp(InTestCase)), ManagedTestCase(InManagedTestCase)
    {
    }
};