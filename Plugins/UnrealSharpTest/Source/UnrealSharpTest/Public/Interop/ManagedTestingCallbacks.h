// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Model/ManagedTestCase.h"

class FCSharpAutomationTest;

struct FManagedTestingActions
{
    using FCollectTestCases = void(__stdcall*)(const FName*, int32, TArray<FManagedTestCaseHandle>*);
    using FGetTests = void(__stdcall*)(FGCHandleIntPtr, TArray<FString>*, TArray<FString>*);
    using FRunTest = bool(__stdcall*)(FCSharpAutomationTest*, FGCHandleIntPtr, FName);
    using FCheckTaskComplete = bool(__stdcall*)(FGCHandleIntPtr);
    using FClearTestClassInstances = void(__stdcall*)();

    FCollectTestCases CollectTestCases = nullptr;
    FGetTests GetTests = nullptr;
    FRunTest RunTest = nullptr;
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
    void GetTests(FGCHandleIntPtr ManagedTest, TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const;
    bool RunTest(FCSharpAutomationTest& Test, FGCHandleIntPtr ManagedTest, FName TestCase) const;
    bool CheckTaskComplete(const FSharedGCHandle& Task) const;
    void ClearTestClassInstances() const;

private:
    FManagedTestingActions Actions;
};
