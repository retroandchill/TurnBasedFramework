// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealSharpTest.h"

#include "CSManager.h"
#include "Interop/ManagedTestingCallbacks.h"

#define LOCTEXT_NAMESPACE "FUnrealSharpTestModule"

FUnrealSharpTestModule* FUnrealSharpTestModule::Instance = nullptr;

void FUnrealSharpTestModule::StartupModule()
{
    Instance = this;

    FCoreDelegates::OnPostEngineInit.AddLambda([this]
    {
        OnAssembliesLoaded();
        UCSManager::Get().OnAssembliesLoadedEvent().AddRaw(this, &FUnrealSharpTestModule::OnAssembliesLoaded);  
    });
}

void FUnrealSharpTestModule::ShutdownModule()
{
    Instance = nullptr;  
}

void FUnrealSharpTestModule::OnAssembliesLoaded()
{
    TestHandles = FManagedTestingCallbacks::Get().GetManagedTests(); 
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FUnrealSharpTestModule, UnrealSharpTest)