// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameDataRepositoryEntrySelector.h"

class FGameDataEntrySerializer;
class UGameDataRepository;

struct FSelectedRow
{
    int32 Index;
    FGameplayTag CurrentName;

    FSelectedRow(const int32 InIndex, const FGameplayTag InCurrentName)
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
    TOptional<FGameplayTag> GenerateUniqueRowName() const;
    bool VerifyRowNameUnique(FGameplayTag Name) const;
    void OnPropertyChanged(const FPropertyChangedEvent& PropertyChangedEvent);
    FGameplayTag GetId(const UObject* Entry) const;
    void SetId(UObject* Entry, const FGameplayTag Id) const;
    int32 GetRowIndex(const UObject* Entry) const;
    void SetRowIndex(UObject* Entry, const int32 Id) const;

    TSharedPtr<SGameDataRepositoryEntrySelector> EntrySelector;
    TSharedPtr<IDetailsView> DetailsView;
    TObjectPtr<UGameDataRepository> GameDataRepository;
    TArray<UObject*>* GameDataEntries = nullptr;
    FStructProperty* IdProperty = nullptr;
    FIntProperty* RowIndexProperty = nullptr;
    TOptional<FSelectedRow> CurrentRow;
    TArray<TSharedRef<FGameDataEntrySerializer>> Serializers;
};
