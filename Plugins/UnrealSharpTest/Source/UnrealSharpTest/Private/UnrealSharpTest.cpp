// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealSharpTest.h"

#include "CSManager.h"
#include "Interop/ManagedTestingCallbacks.h"
#include "Runner/CSharpAutomationTest.h"
#include "UnrealSharpProcHelper/CSProcHelper.h"

#define LOCTEXT_NAMESPACE "FUnrealSharpTestModule"

FUnrealSharpTestModule* FUnrealSharpTestModule::Instance = nullptr;

void FUnrealSharpTestModule::StartupModule()
{
    Instance = this;
    FCoreDelegates::OnAllModuleLoadingPhasesComplete.AddLambda([this]
    {
        RegisterTests();

        auto &Manager = UCSManager::Get();
        Manager.OnAssembliesLoadedEvent().AddRaw(this, &FUnrealSharpTestModule::RegisterTests);
    });
}

void FUnrealSharpTestModule::ShutdownModule()
{
    Instance = nullptr;
}

void FUnrealSharpTestModule::RegisterTests()
{
    auto &TestFramework = FAutomationTestFramework::Get();
    TArray<FString> Paths;
    FCSProcHelper::GetAssemblyPathsByLoadOrder(Paths);
        
    for (auto TestCases = FManagedTestingCallbacks::Get().CollectTestCases(Paths); auto &TestCase : TestCases)
    {
        auto &AssemblyList = Tests.FindOrAdd(TestCase.AssemblyName);
        auto &Test = AssemblyList.Emplace_GetRef(MakeShared<FCSharpAutomationTest>(MoveTemp(TestCase)));
        TestFramework.RegisterAutomationTest(Test->GetTestFullName(), &Test.Get()); 
    }
}

void FUnrealSharpTestModule::UnregisterTests(const FName AssemblyName)
{
    const auto AssemblyList = Tests.Find(AssemblyName);
    if (AssemblyList == nullptr) return;

    auto &TestFramework = FAutomationTestFramework::Get();
    for (const auto &Test : *AssemblyList)
    {
        TestFramework.UnregisterAutomationTest(Test->GetTestFullName());
    }
    Tests.Remove(AssemblyName);
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FUnrealSharpTestModule, UnrealSharpTest)