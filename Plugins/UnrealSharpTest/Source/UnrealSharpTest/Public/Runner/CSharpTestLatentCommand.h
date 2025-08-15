// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSharpAutomationTestBase.h"
#include "ManagedTestListenerHandle.h"

/**
 * 
 */
class UNREALSHARPTEST_API FCSharpTestLatentCommand : public IAutomationLatentCommand
{
public:
    FCSharpTestLatentCommand(FCSharpAutomationTestBase& InTest, FGCHandleIntPtr ManagedListener) : Test(InTest), ListenerHandle(ManagedListener) {}

    void AddInfo(const FString& Message) const
    {
        Test.AddInfo(Message);
    }

    void AddError(const FString& Message) const
    {
        Test.AddError(Message);
    }

private:
    FCSharpAutomationTestBase& Test;
    FManagedTestListenerHandle ListenerHandle;
};
