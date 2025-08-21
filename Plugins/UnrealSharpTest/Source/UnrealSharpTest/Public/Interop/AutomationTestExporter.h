// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "CSManagedGCHandle.h"
#include "UObject/Object.h"
#include "AutomationTestExporter.generated.h"

class FCSharpAutomationTest;
/**
 * 
 */
UCLASS()
class UNREALSHARPTEST_API UAutomationTestExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void AddTestCase(FName ParameterName, const TCHAR* BeautifiedName, TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands);
    
    UNREALSHARP_FUNCTION()
    static void EnqueueLatentCommand(FCSharpAutomationTest* Test, FGCHandleIntPtr TaskPtr);

    UNREALSHARP_FUNCTION()
    static void LogEvent(const TCHAR* Message, EAutomationEventType EventType);
};
