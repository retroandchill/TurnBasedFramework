#pragma once

#include "CoreMinimal.h"
#include "Modules/ModuleManager.h"

class FTurnBasedCoreModule : public IModuleInterface {
public:
  void StartupModule() override;
  void ShutdownModule() override;
};
