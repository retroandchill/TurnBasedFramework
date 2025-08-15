// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingExporter.h"

void UManagedTestingExporter::SetManagedActions(const FManagedTestingActions& InActions)
{
    FManagedTestingCallbacks::Get().SetActions(InActions);  
}

void UManagedTestingExporter::AddTest(TMap<FString, FManagedTestHandle>& Handles, const FGCHandleIntPtr Handle)
{
    FManagedTestHandle TestHandle(Handle);
    auto Name = TestHandle.GetFullyQualifiedName();
    Handles.Emplace(MoveTemp(Name), MoveTemp(TestHandle));  
}
