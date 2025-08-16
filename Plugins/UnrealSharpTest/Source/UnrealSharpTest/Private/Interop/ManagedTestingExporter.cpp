// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ManagedTestingExporter.h"

void UManagedTestingExporter::SetManagedActions(const FManagedTestingActions& InActions)
{
    FManagedTestingCallbacks::Get().SetActions(InActions);  
}

void UManagedTestingExporter::AddTestCase(TArray<FManagedTestCase>& TestCases, FManagedTestCase& TestCase)
{
    TestCases.Add(MoveTemp(TestCase));
}
