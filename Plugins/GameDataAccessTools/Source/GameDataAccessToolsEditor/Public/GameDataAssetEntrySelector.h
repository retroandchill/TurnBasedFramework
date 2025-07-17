// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Widgets/SCompoundWidget.h"
#include "DataRetrieval/GameDataEntry.h"


struct FEntryRowData {
  int32 Index;
  FName Name;
  TWeakObjectPtr<UGameDataEntry> Entry;

  FEntryRowData(const int32 InIndex, const FName InName, UGameDataEntry* InEntry)
    : Index(InIndex), Name(InName), Entry(InEntry) {
  }

  bool operator==(const FEntryRowData& Other) const = default;
};

DECLARE_DELEGATE_OneParam(FOnEntrySelected, const TSharedPtr<FEntryRowData>&);
DECLARE_DELEGATE_RetVal(TArray<TSharedPtr<FEntryRowData>>, FOnGetEntries);
DECLARE_DELEGATE(FOnAddEntry);
DECLARE_DELEGATE_OneParam(FOnDeleteEntry, const TSharedPtr<FEntryRowData>&);
DECLARE_DELEGATE_OneParam(FOnMoveEntryUp, const TSharedPtr<FEntryRowData>&);
DECLARE_DELEGATE_OneParam(FOnMoveEntryDown, const TSharedPtr<FEntryRowData>&);


/**
 *
 */
class GAMEDATAACCESSTOOLSEDITOR_API SGameDataAssetEntrySelector final : public SCompoundWidget {
public:
  SLATE_BEGIN_ARGS(SGameDataAssetEntrySelector) { }
    SLATE_EVENT(FOnEntrySelected, OnEntrySelected)
    SLATE_EVENT(FOnGetEntries, OnGetEntries)
    SLATE_EVENT(FOnAddEntry, OnAddEntry)
    SLATE_EVENT(FOnDeleteEntry, OnDeleteEntry)
    SLATE_EVENT(FOnMoveEntryUp, OnMoveEntryUp)
    SLATE_EVENT(FOnMoveEntryDown, OnMoveEntryDown)
  SLATE_END_ARGS()

  /** Constructs this widget with InArgs */
  void Construct(const FArguments& InArgs);
  void RefreshList();
  void SelectAtIndex(int32 Index);

private:
  TSharedRef<ITableRow> OnGenerateRow(TSharedPtr<FEntryRowData> Item, const TSharedRef<STableViewBase>& OwnerTable);
  void OnSearchTextChanged(const FText& InSearchText);
  void OnSelectionChanged(TSharedPtr<FEntryRowData> Item, ESelectInfo::Type SelectType) const;
  FReply AddEntryClicked() const;
  FReply DeleteEntryClicked() const;
  FReply MoveEntryUp() const;
  FReply MoveEntryDown() const;

  bool CanAddEntry() const;
  bool CanMoveEntryUp() const;
  bool CanMoveEntryDown() const;
  bool CanDeleteEntry() const;

  // UI Elements
  TSharedPtr<SSearchBox> SearchBox;
  TSharedPtr<SListView<TSharedPtr<FEntryRowData>>> EntriesList;

  // Data
  TArray<TSharedPtr<FEntryRowData>> AllEntries;
  TArray<TSharedPtr<FEntryRowData>> FilteredEntries;

  FOnEntrySelected OnEntrySelected;
  FOnGetEntries OnGetEntries;
  FOnAddEntry OnAddEntry;
  FOnDeleteEntry OnDeleteEntry;
  FOnMoveEntryUp OnMoveEntryUp;
  FOnMoveEntryDown OnMoveEntryDown;
};
