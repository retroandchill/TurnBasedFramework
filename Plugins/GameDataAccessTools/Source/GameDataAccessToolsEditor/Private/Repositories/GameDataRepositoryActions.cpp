﻿#include "Repositories/GameDataRepositoryActions.h"

#include "DesktopPlatformModule.h"
#include "EditorDirectories.h"
#include "Repositories/GameDataRepositoryEditor.h"
#include "DataRetrieval/GameDataRepository.h"
#include "Interop/GameDataEntrySerializer.h"
#include "Interop/SerializationCallbacks.h"

FText FGameDataRepositoryActions::GetName() const
{
    return NSLOCTEXT("AssetTypeActions", "AssetTypeActions_GameDataRepository", "Game Data Repository");
}

FColor FGameDataRepositoryActions::GetTypeColor() const
{
    return FColor(140, 80, 200);
}

UClass* FGameDataRepositoryActions::GetSupportedClass() const
{
    return UGameDataRepository::StaticClass();
}

uint32 FGameDataRepositoryActions::GetCategories()
{
    return EAssetTypeCategories::Misc;
}

void FGameDataRepositoryActions::GetActions(const TArray<UObject*>& InObjects, FMenuBuilder& MenuBuilder)
{
    if (InObjects.Num() != 1)
    {
        return;
    }

    MenuBuilder.AddSubMenu(NSLOCTEXT("AssetTypeActions", "Asset_ExportTo", "Export To..."),
                           NSLOCTEXT("AssetTypeActions", "Asset_ExportToTooltip",
                                     "Export the selected asset to different formats"),
                           FNewMenuDelegate::CreateStatic(&FGameDataRepositoryActions::AddSerializationActions,
                                                       const_cast<const UObject*>(InObjects[0])));
}

void FGameDataRepositoryActions::OpenAssetEditor(const TArray<UObject*>& InObjects,
                                                 const TSharedPtr<IToolkitHost> EditWithinLevelEditor)
{
    for (UObject* Object : InObjects)
    {
        if (const auto Asset = Cast<UGameDataRepository>(Object); Asset != nullptr)
        {
            const auto NewEditor = MakeShared<FGameDataRepositoryEditor>();
            NewEditor->Initialize(EToolkitMode::Standalone, EditWithinLevelEditor, Asset);
        }
    }
}

void FGameDataRepositoryActions::AddSerializationActions(FMenuBuilder& SubMenuBuilder, const UObject* InObject)
{
    for (auto Repository = CastChecked<UGameDataRepository>(InObject);
         auto& Serializer : FSerializationCallbacks::Get().GetSerializationActions(Repository->GetEntryClass()))
    {
        const auto FormatText = Serializer->GetFormatName();
        SubMenuBuilder.AddMenuEntry(
            FText::Format(NSLOCTEXT("AssetTypeActions", "Asset_ExportToFormat", "Export as {0}"), FormatText),
            FText::Format(NSLOCTEXT("AssetTypeActions", "Asset_ExportToFormatToolTip", "Export the asset to the {0} format"), FormatText),
            FSlateIcon(),
            FUIAction(
                FExecuteAction::CreateStatic(&FGameDataRepositoryActions::ExportAsset, Repository, Serializer)
            )
        );
    }
}

void FGameDataRepositoryActions::ExportAsset(const UGameDataRepository* Repository,
                                             TSharedRef<FGameDataEntrySerializer> Serializer)
{
    if (IDesktopPlatform* DesktopPlatform = FDesktopPlatformModule::Get(); DesktopPlatform != nullptr)
    {
        const void* ParentWindowWindowHandle = FSlateApplication::Get().FindBestParentWindowHandleForDialogs(nullptr);
        TArray<FString> FileNames;
        if (!DesktopPlatform->SaveFileDialog(
            ParentWindowWindowHandle,
            NSLOCTEXT("GameDataRepository", "Select file to export to", "Select file to export to...").ToString(),
            *FEditorDirectories::Get().GetLastDirectory(ELastDirectory::UNR),
            TEXT(""),
            Serializer->GetFileExtensionText(),
            EFileDialogFlags::None,
            FileNames))
        {
            return;
        }

        const FString& FileName = FileNames[0];
        FEditorDirectories::Get().SetLastDirectory(ELastDirectory::UNR, *FPaths::GetPath(FileName));
        if (auto Serialized = Serializer->SerializeData(Repository); Serialized.has_value())
        {
            FFileHelper::SaveStringToFile(Serialized.value(), *FileName);
            FMessageDialog::Open(EAppMsgType::Ok, NSLOCTEXT("GameDataRepository", "ExportSuccessful", "Export was successful!"));
        }
        else
        {
            FMessageDialog::Open(EAppMsgType::Ok, FText::FromString(MoveTemp(Serialized.error())));
        }
    }
}
