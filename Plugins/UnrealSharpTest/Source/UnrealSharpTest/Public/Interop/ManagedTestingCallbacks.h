// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

struct FManagedTestingActions
{
    using FLoadLoadAssemblyTests = void(__stdcall*)(FName, FGCHandleIntPtr, TArray<FString>*);
    using FUnloadLoadAssemblyTests = void(__stdcall*)(FName);
    using FStartTest = FGCHandleIntPtr(__stdcall*)(FName, const FString*);
    using FCheckTaskComplete = bool(__stdcall*)(FGCHandleIntPtr);

    FLoadLoadAssemblyTests LoadLoadAssemblyTests;
    FUnloadLoadAssemblyTests UnloadLoadAssemblyTests;
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

    TArray<FString> LoadAssemblyTests(const FName AssemblyName, const FGCHandleIntPtr Assembly) const;
    void UnloadAssemblyTests(const FName AssemblyName) const;
    FSharedGCHandle StartTest(const FName AssemblyName, const FString& TestName) const;
    bool CheckTaskComplete(const FSharedGCHandle& Task) const;

private:
    FManagedTestingActions Actions;
};
