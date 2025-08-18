// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Model/ManagedTestCase.h"

class FCSharpAutomationTest;

struct FManagedTestingActions
{
    using FCollectTestCases = void(__stdcall*)(const FName*, int32, TArray<FManagedTestCaseHandle>*);
    using FStartTest = FGCHandleIntPtr(__stdcall*)(const TWeakPtr<FCSharpAutomationTest>*, FGCHandleIntPtr);
    using FCheckTaskComplete = bool(__stdcall*)(FGCHandleIntPtr);
    using FClearTestClassInstances = void(__stdcall*)();

    FCollectTestCases CollectTestCases = nullptr;
    FStartTest StartTest = nullptr;
    FCheckTaskComplete CheckTaskComplete = nullptr;
    FClearTestClassInstances ClearTestClassInstances = nullptr;
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

    bool IsValid() const
    {
        return Actions.CollectTestCases != nullptr;
    }
    
    void SetActions(const FManagedTestingActions& InActions);

    TArray<FManagedTestCaseHandle> CollectTestCases(TConstArrayView<FName> AssemblyPaths) const;
    FSharedGCHandle StartTest(const TWeakPtr<FCSharpAutomationTest>& Test, FGCHandleIntPtr ManagedTest) const;
    bool CheckTaskComplete(const FSharedGCHandle& Task) const;
    void ClearTestClassInstances() const;

private:
    FManagedTestingActions Actions;
};
