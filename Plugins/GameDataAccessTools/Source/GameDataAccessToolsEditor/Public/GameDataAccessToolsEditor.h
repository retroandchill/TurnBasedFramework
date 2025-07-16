#pragma once

#include "CoreMinimal.h"
#include "Modules/ModuleManager.h"

class FGameDataAssetActions;

class FGameDataAccessToolsEditorModule final : public IModuleInterface {
public:
  void StartupModule() override;
  void ShutdownModule() override;

private:
  TSharedPtr<FGameDataAssetActions> GameDataAssetActions;

};
