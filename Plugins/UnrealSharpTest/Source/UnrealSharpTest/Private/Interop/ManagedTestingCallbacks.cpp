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

TArray<FManagedTestCaseHandle> FManagedTestingCallbacks::CollectTestCases(const TConstArrayView<FName> AssemblyPaths) const
{
    TArray<FManagedTestCaseHandle> Result;
    Actions.CollectTestCases(AssemblyPaths.GetData(), AssemblyPaths.Num(), &Result);
    return Result;  
}


FSharedGCHandle FManagedTestingCallbacks::StartTest(const FGCHandleIntPtr ManagedTest) const
{
    return FSharedGCHandle(Actions.StartTest(ManagedTest));
}

bool FManagedTestingCallbacks::CheckTaskComplete(const FSharedGCHandle& Task) const
{
    return Actions.CheckTaskComplete(Task.GetHandle());
}
