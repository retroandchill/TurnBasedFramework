#pragma once

#include "CoreMinimal.h"
#include "Modules/ModuleManager.h"

class FTurnBasedCoreModule final : public IModuleInterface
{
public:
    void StartupModule() override;
    void ShutdownModule() override;
};
