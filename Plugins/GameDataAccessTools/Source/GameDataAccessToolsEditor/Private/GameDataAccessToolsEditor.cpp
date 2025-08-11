#include "GameDataAccessToolsEditor.h"

#include "AssetToolsModule.h"
#include "Repositories/GameDataRepositoryActions.h"
#include "Repositories/GameDataEntryDetailsCustomization.h"
#include "PropertyEditorModule.h"
#include "Handles/DataHandleCustomization.h"
#include "Handles/DataHandlePropertyIdentifier.h"


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

    PropertyModule.RegisterCustomPropertyTypeLayout(
                    "StructProperty",
                    FOnGetPropertyTypeCustomizationInstance::CreateStatic(
                        &FDataHandleCustomization::MakeInstance),
                        MakeShared<FDataHandlePropertyIdentifier>());
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
