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

private:
    TArray<TSharedRef<FCSharpAutomationTest>> Tests;
};
