// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpTestLatentCommand.h"

#include "Interop/ManagedTestingCallbacks.h"


FCSharpTestLatentCommand::FCSharpTestLatentCommand(const FName AssemblyName, const FString& TestName)
    : TestTask(FManagedTestingCallbacks::Get().StartTest(AssemblyName, TestName))
{
    
}

bool FCSharpTestLatentCommand::Update()
{
    return FManagedTestingCallbacks::Get().CheckTaskComplete(TestTask);
}
