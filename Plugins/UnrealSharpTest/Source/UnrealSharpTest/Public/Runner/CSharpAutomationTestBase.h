// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ManagedTestHandle.h"
#include "Interop/ManagedTestingCallbacks.h"

/**
 * 
 */
class UNREALSHARPTEST_API FCSharpAutomationTestBase : public FAutomationTestBase
{
public:
    FCSharpAutomationTestBase(const FString& InName, const bool bInComplexTask)
        : FAutomationTestBase(InName, bInComplexTask)
    {
    }

protected:
    static void CollectCSharpTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands);
};
