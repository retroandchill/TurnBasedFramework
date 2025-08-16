// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingExporter.h"

#include "CSManager.h"

void UManagedTestingExporter::SetManagedActions(const FManagedTestingActions& InActions)
{
    FManagedTestingCallbacks::Get().SetActions(InActions);  
}

void UManagedTestingExporter::AddTest(TArray<FString>& Handles, const TCHAR* TestName)
{
    Handles.Emplace(TestName); 
}

FGCHandleIntPtr UManagedTestingExporter::GetManagedAssembly(const FName AssemblyName)
{
    const auto Assembly = UCSManager::Get().FindAssembly(AssemblyName);
    if (Assembly == nullptr) return FGCHandleIntPtr();
    
    return Assembly->GetManagedAssemblyHandle()->GetHandle();
}

void UManagedTestingExporter::AddTestCase(TArray<FManagedTestCase>& TestCases, FManagedTestCase& TestCase)
{
    TestCases.Add(MoveTemp(TestCase));
}
