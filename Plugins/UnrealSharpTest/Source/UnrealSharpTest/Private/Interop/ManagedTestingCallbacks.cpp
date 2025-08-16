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

TArray<FManagedTestCase> FManagedTestingCallbacks::CollectTestCases(const TConstArrayView<FString> AssemblyPaths) const
{
    TArray<FManagedTestCase> Result;
    Actions.CollectTestCases(AssemblyPaths.GetData(), AssemblyPaths.Num(), &Result);
    return Result;  
}


FSharedGCHandle FManagedTestingCallbacks::StartTest(const FName AssemblyName, const FString& TestName) const
{
    return FSharedGCHandle(Actions.StartTest(AssemblyName, &TestName));
}

bool FManagedTestingCallbacks::CheckTaskComplete(const FSharedGCHandle& Task) const
{
    return Actions.CheckTaskComplete(Task.GetHandle());
}
