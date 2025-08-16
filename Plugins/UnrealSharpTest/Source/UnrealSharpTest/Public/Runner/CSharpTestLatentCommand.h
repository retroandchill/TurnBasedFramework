// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "ManagedTestCase.h"

/**
 * 
 */
class UNREALSHARPTEST_API FCSharpTestLatentCommand final : public IAutomationLatentCommand
{
public:
    explicit FCSharpTestLatentCommand(const FManagedTestCase& TestCase);

    bool Update() override;

private:
    FSharedGCHandle TestTask;
};
