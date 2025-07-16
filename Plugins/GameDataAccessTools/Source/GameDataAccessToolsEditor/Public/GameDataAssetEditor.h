// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Toolkits/AssetEditorToolkit.h"
#include "DataRetrieval/GameDataEntry.h"

class UGameDataAsset;

namespace GameData {
  class SGameDataAssetEditor;

  /**
   * Asset editor for game data assets with entry management
   */
  class GAMEDATAACCESSTOOLSEDITOR_API FGameDataAssetEditor : public FAssetEditorToolkit {
  public:
    void Initialize(EToolkitMode::Type Mode, const TSharedPtr<IToolkitHost>& InitToolkitHost, UGameDataAsset* AssetToEdit);

    FName GetToolkitFName() const override { return FName("GameDataAssetEditor"); }
    FText GetBaseToolkitName() const override { return FText::FromString("Game Data Asset Editor"); }
    FString GetWorldCentricTabPrefix() const override { return FString("GameDataAssetEditor"); }
    FLinearColor GetWorldCentricTabColorScale() const override { return FLinearColor(0.3f, 0.2f, 0.5f, 0.5f); }
    bool IsPrimaryEditor() const override { return true; }

  private:
    /** Spawns the main editor tab */
    TSharedRef<SDockTab> SpawnTab_AssetEditor(const FSpawnTabArgs& Args) const;

    /** Callback when a new entry is added */
    void OnEntryAdded(UGameDataEntry* NewEntry) const;

    /** Callback when an entry is deleted */
    void OnEntryDeleted(int32 Index) const;

    /** Callback when entries are swapped */
    void OnEntriesSwapped(int32 FirstIndex, int32 SecondIndex) const;

    /** Callback to get entries from the asset */
    TArray<TObjectPtr<UGameDataEntry>> GetEntriesFromAsset() const;

    /** Callback when entries are modified */
    void OnEntriesModified() const;

    /** Editor UI */
    TSharedPtr<SGameDataAssetEditor> EditorWidget;

    /** Asset being edited */
    TObjectPtr<UGameDataAsset> EditedAsset;

    TArray<TObjectPtr<UGameDataEntry>>* Entries = nullptr;
  };
}
