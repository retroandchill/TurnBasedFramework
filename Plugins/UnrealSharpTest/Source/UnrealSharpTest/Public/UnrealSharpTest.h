// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "Modules/ModuleManager.h"

class FManagedTestHandle;

class FUnrealSharpTestModule final : public IModuleInterface
{
public:
    void StartupModule() override;
    void ShutdownModule() override;

    static FUnrealSharpTestModule& Get()
    {
        check(Instance != nullptr); 
        return *Instance;  
    }
    const TMap<FString, FManagedTestHandle>& GetTestHandles() const
    {
        return TestHandles;   
    }

private:
    void OnAssembliesLoaded();
    
    static FUnrealSharpTestModule* Instance;
    TMap<FString, FManagedTestHandle> TestHandles;
};
