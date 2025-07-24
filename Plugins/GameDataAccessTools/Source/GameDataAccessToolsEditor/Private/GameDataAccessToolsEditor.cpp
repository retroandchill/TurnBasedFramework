#include "GameDataAccessToolsEditor.h"

#include "AssetToolsModule.h"
#include "GameDataRepositoryActions.h"
#include "GameDataEntryDetailsCustomization.h"
#include "PropertyEditorModule.h"


void FGameDataAccessToolsEditorModule::StartupModule()
{
    // Register the asset type actions
    auto& AssetTools = FModuleManager::LoadModuleChecked<FAssetToolsModule>("AssetTools").Get();

    GameDataRepositoryActions = MakeShared<FGameDataRepositoryActions>();
    AssetTools.RegisterAssetTypeActions(GameDataRepositoryActions.ToSharedRef());

    auto& PropertyModule = FModuleManager::LoadModuleChecked<FPropertyEditorModule>("PropertyEditor");
    PropertyModule.RegisterCustomClassLayout("GameDataEntry",
                                             FOnGetDetailCustomizationInstance::CreateStatic(
                                                 &FGameDataEntryDetailsCustomization::MakeInstance));
}

void FGameDataAccessToolsEditorModule::ShutdownModule()
{
    if (FModuleManager::Get().IsModuleLoaded("AssetTools"))
    {
        IAssetTools& AssetTools = FModuleManager::GetModuleChecked<FAssetToolsModule>("AssetTools").Get();
        if (GameDataRepositoryActions.IsValid())
        {
            AssetTools.UnregisterAssetTypeActions(GameDataRepositoryActions.ToSharedRef());
        }
    }
}

IMPLEMENT_MODULE(FGameDataAccessToolsEditorModule, GameDataAccessToolsEditor)
