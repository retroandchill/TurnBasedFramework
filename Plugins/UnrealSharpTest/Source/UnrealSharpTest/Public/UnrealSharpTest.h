// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "Modules/ModuleManager.h"

class FCSharpAutomationTest;
class FManagedTestHandle;

DECLARE_LOG_CATEGORY_EXTERN(LogUnrealSharpTestNative, Log, All);

class FUnrealSharpTestModule final : public IModuleInterface
{
public:
    void StartupModule() override;
    void ShutdownModule() override;
    
    static FUnrealSharpTestModule& Get()
    {
        return *Instance;   
    }

private:
    void RegisterTests();
    void RegisterTests(const FName& AssemblyName);
    void RegisterTests(TConstArrayView<FName, int> Assemblies);
    void UnregisterTests(const FName& AssemblyName);

    static FUnrealSharpTestModule* Instance;
    TMap<FName, TArray<TSharedRef<FCSharpAutomationTest>>> Tests;
    FDelegateHandle RegisterTestsDelegateHandle;
    FDelegateHandle ClearTestCacheDelegateHandle;
};
