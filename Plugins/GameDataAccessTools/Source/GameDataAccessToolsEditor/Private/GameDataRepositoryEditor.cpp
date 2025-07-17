// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataRepositoryEditor.h"

#include <bit>

#include "GameDataRepositoryEntrySelector.h"
#include "DataRetrieval/GameDataRepository.h"


void FGameDataRepositoryEditor::Initialize(const EToolkitMode::Type Mode, const TSharedPtr<IToolkitHost>& InitToolkitHost,
                                      UGameDataRepository* Asset) {
  GameDataRepository = Asset;

  const auto AssetClass = Asset->GetClass();
  const auto DataEntriesProperty = CastFieldChecked<FArrayProperty>(AssetClass->FindPropertyByName(TEXT("DataEntries")));
  GameDataEntries = std::bit_cast<TArray<UGameDataEntry*>*>(DataEntriesProperty->GetPropertyValuePtr_InContainer(Asset));

  const auto Layout = FTabManager::NewLayout("GameDataRepositoryEditor_Layout")
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

  InitAssetEditor(Mode, InitToolkitHost, "GameDataRepositoryEditor", Layout, true, true, Asset);
}

void FGameDataRepositoryEditor::RegisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) {
  FAssetEditorToolkit::RegisterTabSpawners(InTabManager);

  WorkspaceMenuCategory = InTabManager->AddLocalWorkspaceMenuCategory(NSLOCTEXT("GameDataRepository", "GameDataRepository", "Game Data Asset"));

  InTabManager->RegisterTabSpawner("EntrySelectionTab", FOnSpawnTab::CreateLambda([this](const FSpawnTabArgs&)
  {
    return SNew(SDockTab)
    [
      SAssignNew(EntrySelector, SGameDataRepositoryEntrySelector)
        .OnEntrySelected(this, &FGameDataRepositoryEditor::OnEntrySelected)
        .OnGetEntries(this, &FGameDataRepositoryEditor::OnGetEntries)
        .OnAddEntry(this, &FGameDataRepositoryEditor::OnAddEntry)
        .OnDeleteEntry(this, &FGameDataRepositoryEditor::OnDeleteEntry)
        .OnMoveEntryUp(this, &FGameDataRepositoryEditor::OnMoveEntryUp)
        .OnMoveEntryDown(this, &FGameDataRepositoryEditor::OnMoveEntryDown)
    ];
  }))
  .SetDisplayName(NSLOCTEXT("GameDataRepository", "EntrySelectionTab", "Entries"))
  .SetGroup(WorkspaceMenuCategory.ToSharedRef());

  FPropertyEditorModule& PropertyEditorModule = FModuleManager::GetModuleChecked<FPropertyEditorModule>("PropertyEditor");
  FDetailsViewArgs DetailsViewArgs;
  DetailsViewArgs.NameAreaSettings = FDetailsViewArgs::HideNameArea;
  DetailsView = PropertyEditorModule.CreateDetailView(DetailsViewArgs);
  DetailsView->SetObject(GameDataEntries->Num() > 0 ? (*GameDataEntries)[0] : nullptr);
  DetailsView->OnFinishedChangingProperties().AddRaw(this, &FGameDataRepositoryEditor::OnPropertyChanged);
  InTabManager->RegisterTabSpawner("EntryEditTab", FOnSpawnTab::CreateLambda([this](const FSpawnTabArgs&)
  {
    return SNew(SDockTab)
    [
      DetailsView.ToSharedRef()
    ];
  }))
  .SetDisplayName(NSLOCTEXT("GameDataRepository", "Details", "Details"))
  .SetGroup(WorkspaceMenuCategory.ToSharedRef());
}

void FGameDataRepositoryEditor::UnregisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) {
  FAssetEditorToolkit::UnregisterTabSpawners(InTabManager);
  InTabManager->UnregisterTabSpawner("EntrySelectionTab");
  InTabManager->UnregisterTabSpawner("EntryEditTab");
}

FName FGameDataRepositoryEditor::GetToolkitFName() const {
  return FName("GameDataRepositoryEditor");
}

FText FGameDataRepositoryEditor::GetBaseToolkitName() const {
  return NSLOCTEXT("GameDataRepositoryEditor", "AppLabel", "Game Data Asset Editor");
}

FString FGameDataRepositoryEditor::GetWorldCentricTabPrefix() const {
  return "GameDataRepositoryEditor";
}

FLinearColor FGameDataRepositoryEditor::GetWorldCentricTabColorScale() const {
  return FLinearColor();
}

void FGameDataRepositoryEditor::OnEntrySelected(const TSharedPtr<FEntryRowData>& Entry) {
  if (Entry != nullptr) {
    DetailsView->SetObject(Entry->Entry.Get());
    CurrentRow.Emplace(Entry->Index, Entry->Name);
  } else {
    DetailsView->SetObject(nullptr);
    CurrentRow.Reset();
  }
}

TArray<TSharedPtr<FEntryRowData>> FGameDataRepositoryEditor::OnGetEntries() const {
  TArray<TSharedPtr<FEntryRowData>> Entries;
  for (int32 i = 0; i < GameDataEntries->Num(); i++) {
    auto Entry = (*GameDataEntries)[i];
    Entries.Emplace(MakeShared<FEntryRowData>(i, Entry->Id, Entry));
  }
  return Entries;
}

void FGameDataRepositoryEditor::OnAddEntry() const {
  const auto NewEntry = NewObject<UGameDataEntry>(GameDataRepository, GameDataRepository->GetEntryClass());
  NewEntry->Id = GenerateUniqueRowName();
  NewEntry->RowIndex = GameDataEntries->Num();
  GameDataEntries->Add(NewEntry);
  RefreshList();
}

void FGameDataRepositoryEditor::OnDeleteEntry(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->RemoveAt(Entry->Index);
  if (GameDataEntries->Num() == 0) {
    CurrentRow.Reset();
  } else if (Entry->Index <= GameDataEntries->Num()) {
    int32 NewIndex = GameDataEntries->Num() - 1;

    CurrentRow.Emplace(NewIndex, (*GameDataEntries)[NewIndex]->Id);
  }
  RefreshList();
}

void FGameDataRepositoryEditor::OnMoveEntryUp(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->Swap(Entry->Index, Entry->Index - 1);
  CurrentRow.Emplace(Entry->Index - 1, (*GameDataEntries)[Entry->Index - 1]->Id);
  RefreshList();
}

void FGameDataRepositoryEditor::OnMoveEntryDown(const TSharedPtr<FEntryRowData>& Entry) {
  GameDataEntries->Swap(Entry->Index, Entry->Index + 1);
  CurrentRow.Emplace(Entry->Index + 1, (*GameDataEntries)[Entry->Index + 1]->Id);
  RefreshList();
}

void FGameDataRepositoryEditor::RefreshList() const {
  for (int32 i = 0; i < GameDataEntries->Num(); i++) {
    const auto Entry = (*GameDataEntries)[i];
    Entry->RowIndex = i;
  }
  GameDataRepository->Modify();
  EntrySelector->RefreshList();
  if (CurrentRow.IsSet()) {
    EntrySelector->SelectAtIndex(CurrentRow.GetValue().Index);
  }
}

FName FGameDataRepositoryEditor::GenerateUniqueRowName() const {
  TSet<FName> UsedNames;
  for (const auto& Entry : *GameDataEntries) {
    UsedNames.Add(Entry->GetId());
  }

  FName Name = FName("Entry");
  int32 Index = 0;
  while (UsedNames.Contains(Name)) {
    Name = FName(*FString::Printf(TEXT("Entry%d"), Index));
    checkf(Index != std::numeric_limits<int32>::max(), TEXT("Index is about to overflow. Too many entries added."));
    ++Index;
  }

  return Name;
}

bool FGameDataRepositoryEditor::VerifyRowNameUnique(FName Name) const {
  bool bFirstInstance = false;
  for (const auto& Entry : *GameDataEntries) {
    if (Entry->GetId() == Name) {
      if (!bFirstInstance) {
        bFirstInstance = true;
        continue;
      }
      return false;
    }
  }

  return true;
}

void FGameDataRepositoryEditor::OnPropertyChanged(const FPropertyChangedEvent& PropertyChangedEvent) {
  if (PropertyChangedEvent.GetPropertyName() != GET_MEMBER_NAME_CHECKED(UGameDataEntry, Id)) {
    return;
  }

  check(CurrentRow.IsSet());
  auto &[Index, CurrentName] = CurrentRow.GetValue();
  if (auto SelectedEntry = (*GameDataEntries)[Index]; SelectedEntry->Id.IsNone() || !VerifyRowNameUnique(SelectedEntry->Id)) {
    SelectedEntry->Id = CurrentName;
  }
  RefreshList();
}
