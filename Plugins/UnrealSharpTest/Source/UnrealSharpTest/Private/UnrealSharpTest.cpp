// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealSharpTest.h"

#include "CSManager.h"
#include "Interop/ManagedTestingCallbacks.h"
#include "Runner/CSharpAutomationTest.h"
#include "UnrealSharpProcHelper/CSProcHelper.h"

#define LOCTEXT_NAMESPACE "FUnrealSharpTestModule"

void FUnrealSharpTestModule::StartupModule()
{
    FCoreDelegates::OnAllModuleLoadingPhasesComplete.AddLambda([this]
    {
        auto& TestFramework = FAutomationTestFramework::Get();
        for (const auto &Test : Tests)
        {
            TestFramework.UnregisterAutomationTest(Test->GetTestFullName());
        }
        Tests.Reset();
        
        TArray<FString> Paths;
        FCSProcHelper::GetAssemblyPathsByLoadOrder(Paths);
        
        for (auto TestCases = FManagedTestingCallbacks::Get().CollectTestCases(Paths); auto &TestCase : TestCases)
        {
            auto &Test = Tests.Emplace_GetRef(MakeShared<FCSharpAutomationTest>(MoveTemp(TestCase)));
            TestFramework.RegisterAutomationTest(Test->GetTestFullName(), &Test.Get()); 
        }
    });
}

void FUnrealSharpTestModule::ShutdownModule()
{
}

#undef LOCTEXT_NAMESPACE
	
IMPLEMENT_MODULE(FUnrealSharpTestModule, UnrealSharpTest)