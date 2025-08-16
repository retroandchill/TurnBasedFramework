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
        auto& Manager = UCSManager::Get();
        Manager.ForEachManagedAssembly([this](const FName Name, const TSharedPtr<FCSAssembly>&)
        {
            OnAssemblyLoaded(Name);
        });
        Manager.OnManagedAssemblyLoadedEvent().AddRaw(this, &FUnrealSharpTestModule::OnAssemblyLoaded);
    });
}

void FUnrealSharpTestModule::ShutdownModule()
{
    Instance = nullptr;  
}

void FUnrealSharpTestModule::OnAssemblyLoaded(const FName &AssemblyName)
{
    const auto &Callbacks = FManagedTestingCallbacks::Get();
    const auto Assembly = UCSManager::Get().FindAssembly(AssemblyName);
    TestIds.Add(AssemblyName, Callbacks.LoadAssemblyTests(AssemblyName, Assembly->GetManagedAssemblyHandle()->Handle));
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FUnrealSharpTestModule, UnrealSharpTest)