// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Runner/ManagedTestHandle.h"

struct FGCHandleIntPtr;

struct FManagedTestingActions
{
    using FGetManagedTests = void(__stdcall*)(TMap<FString, FManagedTestHandle>*);
    using FGetFullyQualifiedName = void(__stdcall*)(const FGCHandleIntPtr, FString*);

    FGetManagedTests GetManagedTests;
    FGetFullyQualifiedName GetFullyQualifiedName;
};

/**
 * 
 */
class UNREALSHARPTEST_API FManagedTestingCallbacks
{
    FManagedTestingCallbacks() = default;
    ~FManagedTestingCallbacks() = default;

public:
    static FManagedTestingCallbacks& Get();

    void SetActions(const FManagedTestingActions& InActions);

    TMap<FString, FManagedTestHandle> GetManagedTests() const;
    FString GetFullyQualifiedName(const FGCHandleIntPtr& Handle) const;

private:
    FManagedTestingActions Actions;
};
