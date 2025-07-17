// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameDataAssetEntrySelector.h"
#include "DataRetrieval/GameDataEntry.h"

class UGameDataAsset;
/**
 *
 */
class GAMEDATAACCESSTOOLSEDITOR_API FGameDataAssetEditor final : public FAssetEditorToolkit
 {
public:
  void Initialize(const EToolkitMode::Type Mode, const TSharedPtr<IToolkitHost>& InitToolkitHost, UGameDataAsset* Asset);
  void RegisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) override;
  void UnregisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) override;

  FName GetToolkitFName() const override;
  FText GetBaseToolkitName() const override;
  FString GetWorldCentricTabPrefix() const override;
  FLinearColor GetWorldCentricTabColorScale() const override;

private:
  void OnEntrySelected(const TSharedPtr<FEntryRowData>& Entry);
  TArray<TSharedPtr<FEntryRowData>> OnGetEntries();
  void OnAddEntry() const;
  void OnDeleteEntry(const TSharedPtr<FEntryRowData>& Entry);
  void OnMoveEntryUp(const TSharedPtr<FEntryRowData>& Entry);
  void OnMoveEntryDown(const TSharedPtr<FEntryRowData>& Entry);
  void RefreshList() const;
  FName GenerateUniqueRowName() const;
  bool VerifyRowNameUnique(FName Name) const;

  TSharedPtr<SGameDataAssetEntrySelector> EntrySelector;
  TSharedPtr<IDetailsView> DetailsView;
  TObjectPtr<UGameDataAsset> GameDataAsset;
  TArray<UGameDataEntry*>* GameDataEntries = nullptr;
  TOptional<int32> CurrentRowIndex;

};
