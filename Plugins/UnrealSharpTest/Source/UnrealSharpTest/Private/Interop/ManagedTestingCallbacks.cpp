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

TMap<FString, FManagedTestHandle> FManagedTestingCallbacks::GetManagedTests() const
{
    TMap<FString, FManagedTestHandle> Result;
    Actions.GetManagedTests(&Result);
    return Result;  
}

FString FManagedTestingCallbacks::GetFullyQualifiedName(const FGCHandleIntPtr& Handle) const
{
    FString Result;
    Actions.GetFullyQualifiedName(Handle, &Result);
    return Result;
}
