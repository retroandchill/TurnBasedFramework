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
    
    const TMap<FName, TArray<FString>>& GetTestIds() const
    {
        return TestIds;   
    }

private:
    void OnAssemblyLoaded(const FName &AssemblyName);
    
    static FUnrealSharpTestModule* Instance;
    TMap<FName, TArray<FString>> TestIds;
};
