// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Model/ManagedTestCase.h"

struct FManagedTestingActions
{
    using FCollectTestCases = void(__stdcall*)(const FString*, int32, TArray<FManagedTestCaseHandle>*);
    using FLoadLoadAssemblyTests = void(__stdcall*)(FName, FGCHandleIntPtr, TArray<FString>*);
    using FStartTest = FGCHandleIntPtr(__stdcall*)(FGCHandleIntPtr);
    using FCheckTaskComplete = bool(__stdcall*)(FGCHandleIntPtr);

    FCollectTestCases CollectTestCases;
    FStartTest StartTest;
    FCheckTaskComplete CheckTaskComplete;
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

    TArray<FManagedTestCaseHandle> CollectTestCases(TConstArrayView<FString> AssemblyPaths) const;
    FSharedGCHandle StartTest(FGCHandleIntPtr ManagedTest) const;
    bool CheckTaskComplete(const FSharedGCHandle& Task) const;

private:
    FManagedTestingActions Actions;
};
