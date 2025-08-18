// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpTestLatentCommand.h"

#include "Interop/ManagedTestingCallbacks.h"

FCSharpTestLatentCommand::FCSharpTestLatentCommand(TWeakPtr<FCSharpAutomationTest> InOwner, const FGCHandleIntPtr TestCase)
    : Owner(MoveTemp(InOwner)), TestTask(FManagedTestingCallbacks::Get().StartTest(Owner, TestCase))
{
}

bool FCSharpTestLatentCommand::Update()
{
    return FManagedTestingCallbacks::Get().CheckTaskComplete(TestTask);
}
