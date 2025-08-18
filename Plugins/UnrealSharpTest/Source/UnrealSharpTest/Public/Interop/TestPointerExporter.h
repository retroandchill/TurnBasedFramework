// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "TestPointerExporter.generated.h"

/**
 * 
 */
UCLASS()
class UNREALSHARPTEST_API UTestPointerExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void CopyWeakPtr(const TWeakPtr<FAutomationTestBase>& Source, TWeakPtr<FAutomationTestBase>& Dest);

    UNREALSHARP_FUNCTION()
    static void ReleaseWeakPtr(TWeakPtr<FAutomationTestBase>& Source);
};
