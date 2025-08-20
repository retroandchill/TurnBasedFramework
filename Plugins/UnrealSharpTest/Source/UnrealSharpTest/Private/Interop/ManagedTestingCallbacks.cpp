// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingCallbacks.h"

#include "Runner/CSharpAutomationTest.h"


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

bool FManagedTestingCallbacks::RunTest(FCSharpAutomationTest& Test, FGCHandleIntPtr ManagedTest) const
{
    return Actions.RunTest(&Test, ManagedTest);
}

bool FManagedTestingCallbacks::CheckTaskComplete(const FSharedGCHandle& Task) const
{
    return Actions.CheckTaskComplete(Task.GetHandle());
}

void FManagedTestingCallbacks::ClearTestClassInstances() const
{
    Actions.ClearTestClassInstances(); 
}
