// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingCallbacks.h"


FManagedTestingCallbacks& FManagedTestingCallbacks::Get()
{
    static FManagedTestingCallbacks Instance;
    return Instance;
}

void FManagedTestingCallbacks::SetActions(const FManagedTestingActions& InActions)
{
    Actions = InActions;   
}

TArray<FString> FManagedTestingCallbacks::LoadAssemblyTests(const FName AssemblyName, const FGCHandleIntPtr Assembly) const
{
    TArray<FString> Result;
    Actions.LoadLoadAssemblyTests(AssemblyName, Assembly, &Result);
    return Result; 
}

void FManagedTestingCallbacks::UnloadAssemblyTests(const FName AssemblyName) const
{
    Actions.UnloadLoadAssemblyTests(AssemblyName); 
}

FSharedGCHandle FManagedTestingCallbacks::StartTest(const FName AssemblyName, const FString& TestName) const
{
    return FSharedGCHandle(Actions.StartTest(AssemblyName, &TestName));
}

bool FManagedTestingCallbacks::CheckTaskComplete(const FSharedGCHandle& Task) const
{
    return Actions.CheckTaskComplete(Task.GetHandle());
}
