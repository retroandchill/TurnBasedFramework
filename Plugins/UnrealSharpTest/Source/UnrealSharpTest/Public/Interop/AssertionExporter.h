// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "AssertionExporter.generated.h"

class FCSharpAutomationTest;
/**
 * 
 */
UCLASS()
class UNREALSHARPTEST_API UAssertionExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void RecordAssertion(const TWeakPtr<FCSharpAutomationTest>& Test, const TCHAR* Message, EAutomationEventType Severity, const TCHAR* Filename, int32
                                LineNumber);
};
