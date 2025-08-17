// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ManagedTestingCallbacks.h"
#include "Model/ManagedTestCase.h"
#include "UnrealSharpBinds/Public/CSBindsManager.h"
#include "UObject/Object.h"
#include "ManagedTestingExporter.generated.h"

/**
 * 
 */
UCLASS()
class UNREALSHARPTEST_API UManagedTestingExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void SetManagedActions(const FManagedTestingActions& InActions);

    UNREALSHARP_FUNCTION()
    static FGCHandleIntPtr FindUserAssembly(FName AssemblyName);

    UNREALSHARP_FUNCTION()
    static void AddTestCase(TArray<FManagedTestCaseHandle>& TestCases, FManagedTestCase& TestCase, FGCHandleIntPtr ManagedTest);

    UNREALSHARP_FUNCTION()
    static void RemoveTestCasesForAssembly(FName AssemblyName);
};
