// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameDataRepositoryEntrySelector.h"
#include "DataRetrieval/GameDataEntry.h"

class FGameDataEntrySerializer;
class UGameDataRepository;

struct FSelectedRow
{
    int32 Index;
    FName CurrentName;

    FSelectedRow(int32 InIndex, FName InCurrentName)
        : Index(InIndex), CurrentName(InCurrentName)
    {
    }
};

/**
 *
 */
class GAMEDATAACCESSTOOLSEDITOR_API FGameDataRepositoryEditor final : public FAssetEditorToolkit
{
public:
    void Initialize(const EToolkitMode::Type Mode, const TSharedPtr<IToolkitHost>& InitToolkitHost,
                    UGameDataRepository* Asset);
    void RegisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) override;
    void UnregisterTabSpawners(const TSharedRef<FTabManager>& InTabManager) override;

    FName GetToolkitFName() const override;
    FText GetBaseToolkitName() const override;
    FString GetWorldCentricTabPrefix() const override;
    FLinearColor GetWorldCentricTabColorScale() const override;

private:
    void FillToolbar(FToolBarBuilder& ToolbarBuilder);

    TSharedRef<SWidget> ImportMenuEntries();
    void ImportGameDataRepository(TSharedRef<FGameDataEntrySerializer> Serializer);

    bool CanAddEntry() const;
    bool CanMoveEntryUp() const;
    bool CanMoveEntryDown() const;
    bool CanDeleteEntry() const;
    
    void OnEntrySelected(const TSharedPtr<FEntryRowData>& Entry);
    TArray<TSharedPtr<FEntryRowData>> OnGetEntries() const;
    void OnAddEntry() const;
    void OnDeleteEntry();
    void OnMoveEntryUp();
    void OnMoveEntryDown();
    void RefreshList() const;
    FName GenerateUniqueRowName() const;
    bool VerifyRowNameUnique(FName Name) const;
    void OnPropertyChanged(const FPropertyChangedEvent& PropertyChangedEvent);

    TSharedPtr<SGameDataRepositoryEntrySelector> EntrySelector;
    TSharedPtr<IDetailsView> DetailsView;
    TObjectPtr<UGameDataRepository> GameDataRepository;
    TArray<UGameDataEntry*>* GameDataEntries = nullptr;
    TOptional<FSelectedRow> CurrentRow;
    TArray<TSharedRef<FGameDataEntrySerializer>> Serializers;
};
