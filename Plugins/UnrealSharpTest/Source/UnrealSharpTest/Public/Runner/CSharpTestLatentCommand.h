// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Model/ManagedTestCase.h"

class FCSharpAutomationTest;
/**
 * 
 */
class UNREALSHARPTEST_API FCSharpTestLatentCommand final : public IAutomationLatentCommand
{
public:
    FCSharpTestLatentCommand(TWeakPtr<FCSharpAutomationTest> Owner, FGCHandleIntPtr TestTask);

    bool Update() override;

private:
    TWeakPtr<FCSharpAutomationTest> Owner;
    FSharedGCHandle TestTask;
};
