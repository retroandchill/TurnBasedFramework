// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingExporter.h"

void UManagedTestingExporter::SetManagedActions(const FManagedTestingActions& InActions)
{
    FManagedTestingCallbacks::Get().SetActions(InActions);  
}

void UManagedTestingExporter::AddTest(TArray<FString>& Handles, const TCHAR* TestName)
{
    Handles.Emplace(TestName); 
}
