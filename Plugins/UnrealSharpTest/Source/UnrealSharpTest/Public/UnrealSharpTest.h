// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "Modules/ModuleManager.h"

class FCSharpAutomationTest;
class FManagedTestHandle;

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
    void UnregisterTests(FName AssemblyName);

    static FUnrealSharpTestModule* Instance;
    TMap<FName, TArray<TSharedRef<FCSharpAutomationTest>>> Tests;

    friend class UManagedTestingExporter;
};
