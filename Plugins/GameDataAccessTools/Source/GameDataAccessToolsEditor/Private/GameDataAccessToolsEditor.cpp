#include "GameDataAccessToolsEditor.h"

#include "AssetToolsModule.h"
#include "GameDataAssetActions.h"


void FGameDataAccessToolsEditorModule::StartupModule() {
  // Register the asset type actions
  auto& AssetTools = FModuleManager::LoadModuleChecked<FAssetToolsModule>("AssetTools").Get();

  GameDataAssetActions = MakeShared<FGameDataAssetActions>();
  AssetTools.RegisterAssetTypeActions(GameDataAssetActions.ToSharedRef());
}

void FGameDataAccessToolsEditorModule::ShutdownModule() {
  if (FModuleManager::Get().IsModuleLoaded("AssetTools")) {
    IAssetTools& AssetTools = FModuleManager::GetModuleChecked<FAssetToolsModule>("AssetTools").Get();
    if (GameDataAssetActions.IsValid()) {
      AssetTools.UnregisterAssetTypeActions(GameDataAssetActions.ToSharedRef());
    }
  }
}

IMPLEMENT_MODULE(FGameDataAccessToolsEditorModule, GameDataAccessToolsEditor)
