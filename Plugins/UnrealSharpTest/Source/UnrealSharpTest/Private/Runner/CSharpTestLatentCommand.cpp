// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpTestLatentCommand.h"

#include "Interop/ManagedTestingCallbacks.h"

FCSharpTestLatentCommand::FCSharpTestLatentCommand(const FManagedTestCase& TestCase)
    : TestTask(FManagedTestingCallbacks::Get().StartTest(TestCase))
{
}

bool FCSharpTestLatentCommand::Update()
{
    return FManagedTestingCallbacks::Get().CheckTaskComplete(TestTask);
}
