// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataAssetEditor.h"

#include <bit>

#include "SGameDataAssetEditor.h"
#include "DataRetrieval/GameDataEntry.h"
#include "Widgets/Docking/SDockTab.h"
#include "WorkspaceMenuStructure.h"
#include "WorkspaceMenuStructureModule.h"
#include "DataRetrieval/GameDataAsset.h"

#define LOCTEXT_NAMESPACE "GameDataAssetEditor"

namespace GameData {
  static const FName GameDataAssetEditorTabId(TEXT("GameDataAssetEditor_DataEditor"));

  void FGameDataAssetEditor::Initialize(EToolkitMode::Type Mode,
    const TSharedPtr<IToolkitHost>& InitToolkitHost, UGameDataAsset* AssetToEdit) {
    static FName PropertyName = TEXT("DataEntries");
    EditedAsset = AssetToEdit;

    auto DataClass = EditedAsset->GetEntryClass();
    check(DataClass != nullptr);

    auto EntriesProperty = CastFieldChecked<FArrayProperty>(DataClass->FindPropertyByName(PropertyName));
    auto ScriptArray = &EntriesProperty->GetPropertyValue_InContainer(EditedAsset);

    Entries = std::bit_cast<TArray<TObjectPtr<UGameDataEntry>> *>(&ScriptArray);

    auto StandaloneDefaultLayout = FTabManager::NewLayout("Standalone_GameDataAssetEditor_Layout_v1")->
      AddArea(
        FTabManager::NewPrimaryArea()->SetOrientation(Orient_Vertical)->Split(
          FTabManager::NewStack()->AddTab(GameDataAssetEditorTabId, ETabState::OpenedTab)
        )
      );

    // Initialize the asset editor
    InitAssetEditor(
      Mode,
      InitToolkitHost,
      FName("GameDataAssetEditorApp"),
      StandaloneDefaultLayout,
      true,
      true,
      AssetToEdit
    );

    // Create the editor widget
    EditorWidget = SNew(SGameDataAssetEditor)
      .DataAsset(EditedAsset)
      .OnEntryAdded(FOnEntryAddedDelegate::CreateSP(this, &FGameDataAssetEditor::OnEntryAdded))
      .OnEntryDeleted(FOnEntryDeletedDelegate::CreateSP(this, &FGameDataAssetEditor::OnEntryDeleted))
      .OnEntriesSwapped(FOnEntriesSwappedDelegate::CreateSP(this, &FGameDataAssetEditor::OnEntriesSwapped))
      .OnGetEntries(FOnGetEntriesDelegate::CreateSP(this, &FGameDataAssetEditor::GetEntriesFromAsset))
      .OnEntriesModified(FOnEntriesModifiedDelegate::CreateSP(this, &FGameDataAssetEditor::OnEntriesModified));

    // Register tab spawners
    auto TabManager = GetTabManager();

    TabManager->RegisterTabSpawner(
      GameDataAssetEditorTabId,
      FOnSpawnTab::CreateSP(this, &FGameDataAssetEditor::SpawnTab_AssetEditor)
    )
    .SetDisplayName(LOCTEXT("DataEditorTabTitle", "Data Editor"))
    .SetGroup(WorkspaceMenu::GetMenuStructure().GetLevelEditorCategory());
  }

  TSharedRef<SDockTab> FGameDataAssetEditor::SpawnTab_AssetEditor(const FSpawnTabArgs&) const {
    return SNew(SDockTab)
      .Label(LOCTEXT("DataEditorTitle", "Data Editor"))
      [
        EditorWidget.ToSharedRef()
      ];
  }

  void FGameDataAssetEditor::OnEntryAdded(UGameDataEntry* NewEntry) const {
    Entries->Add(NewEntry);

    // Mark the asset as dirty
    EditedAsset->Modify();
  }

  void FGameDataAssetEditor::OnEntryDeleted(int32 Index) const {
    Entries->RemoveAt(Index);

    // Mark the asset as dirty
    EditedAsset->Modify();
  }

  void FGameDataAssetEditor::OnEntriesSwapped(int32 FirstIndex, int32 SecondIndex) const {
    Entries->Swap(FirstIndex, SecondIndex);

    // Mark the asset as dirty
    EditedAsset->Modify();
  }

  TArray<TObjectPtr<UGameDataEntry>> FGameDataAssetEditor::GetEntriesFromAsset() const {
    return *Entries;
  }

  void FGameDataAssetEditor::OnEntriesModified() const {
    for (int32 i = 0; i < Entries->Num(); i++) {
      auto Entry = (*Entries)[i];
      Entry->RowIndex = i;
    }

    // Mark the asset as dirty
    if (EditedAsset) {
      EditedAsset->Modify();
    }
  }
}

#undef LOCTEXT_NAMESPACE
