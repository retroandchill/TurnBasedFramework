// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataAssetEditor.h"

#include <bit>

#include "GameDataAssetEntrySelector.h"
#include "DataRetrieval/GameDataAsset.h"


void FGameDataAssetEditor::Initialize(const EToolkitMode::Type Mode, const TSharedPtr<IToolkitHost>& InitToolkitHost,
                                      UGameDataAsset* Asset) {
  GameDataAsset = Asset;

  const auto AssetClass = Asset->GetClass();
  const auto DataEntriesProperty = CastFieldChecked<FArrayProperty>(AssetClass->FindPropertyByName(TEXT("DataEntries")));
  GameDataEntries = std::bit_cast<TArray<UGameDataEntry*>*>(DataEntriesProperty->GetPropertyValuePtr_InContainer(Asset));

  const auto Layout = FTabManager::NewLayout("GameDataAssetEditor_Layout")
    ->AddArea
    (
      FTabManager::NewPrimaryArea()->SetOrientation(Orient_Horizontal)
      ->Split
      (
      FTabManager::NewStack()
        ->SetSizeCoefficient(0.3f)
        ->AddTab("EntrySelectionTab", ETabState::OpenedTab)
      )
      ->Split
      (
        FTabManager::NewStack()
        ->SetSizeCoefficient(0.7f)
        ->AddTab("EntryEditTab", ETabState::OpenedTab)
      )
    );

  InitAssetEditor(Mode, InitToolkitHost, "GameDataAssetEditor", Layout, true, true, Asset);
}

void FGameDataAssetEditor::RegisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) {
  FAssetEditorToolkit::RegisterTabSpawners(InTabManager);

  WorkspaceMenuCategory = InTabManager->AddLocalWorkspaceMenuCategory(NSLOCTEXT("GameDataAsset", "GameDataAsset", "Game Data Asset"));

  InTabManager->RegisterTabSpawner("EntrySelectionTab", FOnSpawnTab::CreateLambda([this](const FSpawnTabArgs&)
  {
    return SNew(SDockTab)
    [
      SAssignNew(EntrySelector, SGameDataAssetEntrySelector)
        .OnEntrySelected(this, &FGameDataAssetEditor::OnEntrySelected)
        .OnGetEntries(this, &FGameDataAssetEditor::OnGetEntries)
        .OnAddEntry(this, &FGameDataAssetEditor::OnAddEntry)
        .OnDeleteEntry(this, &FGameDataAssetEditor::OnDeleteEntry)
        .OnMoveEntryUp(this, &FGameDataAssetEditor::OnMoveEntryUp)
        .OnMoveEntryDown(this, &FGameDataAssetEditor::OnMoveEntryDown)
    ];
  }))
  .SetDisplayName(NSLOCTEXT("GameDataAsset", "EntrySelectionTab", "Entries"))
  .SetGroup(WorkspaceMenuCategory.ToSharedRef());

  FPropertyEditorModule& PropertyEditorModule = FModuleManager::GetModuleChecked<FPropertyEditorModule>("PropertyEditor");
  FDetailsViewArgs DetailsViewArgs;
  DetailsViewArgs.NameAreaSettings = FDetailsViewArgs::HideNameArea;
  DetailsView = PropertyEditorModule.CreateDetailView(DetailsViewArgs);
  DetailsView->SetObject(GameDataEntries->Num() > 0 ? (*GameDataEntries)[0] : nullptr);
  InTabManager->RegisterTabSpawner("EntryEditTab", FOnSpawnTab::CreateLambda([this](const FSpawnTabArgs&)
  {
    return SNew(SDockTab)
    [
      DetailsView.ToSharedRef()
    ];
  }))
  .SetDisplayName(NSLOCTEXT("GameDataAsset", "Details", "Details"))
  .SetGroup(WorkspaceMenuCategory.ToSharedRef());
}

void FGameDataAssetEditor::UnregisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) {
  FAssetEditorToolkit::UnregisterTabSpawners(InTabManager);
  InTabManager->UnregisterTabSpawner("EntrySelectionTab");
  InTabManager->UnregisterTabSpawner("EntryEditTab");
}

FName FGameDataAssetEditor::GetToolkitFName() const {
  return FName("GameDataAssetEditor");
}

FText FGameDataAssetEditor::GetBaseToolkitName() const {
  return NSLOCTEXT("GameDataAssetEditor", "AppLabel", "Game Data Asset Editor");
}

FString FGameDataAssetEditor::GetWorldCentricTabPrefix() const {
  return "GameDataAssetEditor";
}

FLinearColor FGameDataAssetEditor::GetWorldCentricTabColorScale() const {
  return FLinearColor();
}

void FGameDataAssetEditor::OnEntrySelected(const TSharedPtr<FEntryRowData>& Entry) {
  if (Entry != nullptr) {
    DetailsView->SetObject(Entry->Entry.Get());
    CurrentRowIndex.Emplace(Entry->Index);
  } else {
    DetailsView->SetObject(nullptr);
    CurrentRowIndex.Reset();
  }
}

TArray<TSharedPtr<FEntryRowData>> FGameDataAssetEditor::OnGetEntries() {
  TArray<TSharedPtr<FEntryRowData>> Entries;
  for (int32 i = 0; i < GameDataEntries->Num(); i++) {
    auto Entry = (*GameDataEntries)[i];
    Entries.Emplace(MakeShared<FEntryRowData>(i, Entry->GetId(), Entry));
  }
  return Entries;
}

void FGameDataAssetEditor::OnAddEntry() const {
  const auto NewEntry = NewObject<UGameDataEntry>(GameDataAsset, GameDataAsset->GetEntryClass());
  NewEntry->RowIndex = GameDataEntries->Num();
  GameDataEntries->Add(NewEntry);
  RefreshList();
}

void FGameDataAssetEditor::OnDeleteEntry(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->RemoveAt(Entry->Index);
  if (GameDataEntries->Num() == 0) {
    CurrentRowIndex.Reset();
  } else if (Entry->Index <= GameDataEntries->Num()) {
    CurrentRowIndex.Emplace(GameDataEntries->Num() - 1);
  }
  RefreshList();
}

void FGameDataAssetEditor::OnMoveEntryUp(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->Swap(Entry->Index, Entry->Index - 1);
  CurrentRowIndex.Emplace(Entry->Index - 1);
  RefreshList();
}

void FGameDataAssetEditor::OnMoveEntryDown(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->Swap(Entry->Index, Entry->Index + 1);
  CurrentRowIndex.Emplace(Entry->Index + 1);
  RefreshList();
}

void FGameDataAssetEditor::RefreshList() const {
  for (int32 i = 0; i < GameDataEntries->Num(); i++) {
    const auto Entry = (*GameDataEntries)[i];
    Entry->RowIndex = i;
  }
  GameDataAsset->Modify();
  EntrySelector->RefreshList();
  if (CurrentRowIndex.IsSet()) {
    EntrySelector->SelectAtIndex(CurrentRowIndex.GetValue());
  }
}
