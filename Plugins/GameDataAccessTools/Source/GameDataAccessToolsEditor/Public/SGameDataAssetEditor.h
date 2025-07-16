// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Widgets/SCompoundWidget.h"
#include "Widgets/Views/SListView.h"
#include "DataRetrieval/GameDataEntry.h"

class UGameDataAsset;
class UCSPrimaryDataAsset;

namespace GameData {
  // Forward declarations for delegate types
  DECLARE_DELEGATE_OneParam(FOnEntryAddedDelegate, UGameDataEntry*);
  DECLARE_DELEGATE_OneParam(FOnEntryDeletedDelegate, int32);
  DECLARE_DELEGATE_TwoParams(FOnEntriesSwappedDelegate, int32, int32);
  DECLARE_DELEGATE_RetVal(TArray<TObjectPtr<UGameDataEntry>>, FOnGetEntriesDelegate);
  DECLARE_DELEGATE(FOnEntriesModifiedDelegate);

  /**
   * Widget that provides a split view editor for Game Data Assets.
   * Left side shows a list of entries, right side shows details of the selected entry.
   */
  class GAMEDATAACCESSTOOLSEDITOR_API SGameDataAssetEditor : public SCompoundWidget {
  public:
    SLATE_BEGIN_ARGS(SGameDataAssetEditor) {
    }

    SLATE_ARGUMENT(UGameDataAsset*, DataAsset)
    SLATE_ARGUMENT(FOnEntryAddedDelegate, OnEntryAdded)
    SLATE_ARGUMENT(FOnEntryDeletedDelegate, OnEntryDeleted)
    SLATE_ARGUMENT(FOnEntriesSwappedDelegate, OnEntriesSwapped)
    SLATE_ARGUMENT(FOnGetEntriesDelegate, OnGetEntries)
    SLATE_ARGUMENT(FOnEntriesModifiedDelegate, OnEntriesModified)
    SLATE_END_ARGS()

    void Construct(const FArguments& InArgs);

    /** Refreshes the entry list view */
    void RefreshEntryList();

  private:
    /** Generates a row widget for an entry in the list */
    TSharedRef<ITableRow> OnGenerateEntryRow(TObjectPtr<UGameDataEntry> InEntry, const TSharedRef<STableViewBase>& OwnerTable);

    /** Called when an entry is selected in the list */
    void OnEntrySelectionChanged(TObjectPtr<UGameDataEntry> InEntry, ESelectInfo::Type SelectInfo);

    /** Creates a new entry */
    FReply OnAddEntryClicked();

    /** Deletes the selected entry */
    FReply OnDeleteEntryClicked();

    /** Moves the selected entry up in the list */
    FReply OnMoveEntryUpClicked();

    /** Moves the selected entry down in the list */
    FReply OnMoveEntryDownClicked();


    /** The asset being edited */
    TObjectPtr<UGameDataAsset> DataAsset;

    /** Entry list */
    TArray<TObjectPtr<UGameDataEntry>> Entries;

    /** The list view widget */
    TSharedPtr<SListView<TObjectPtr<UGameDataEntry>>> EntryListView;

    /** The details view */
    TSharedPtr<class IDetailsView> EntryDetailsView;

    /** Currently selected entry */
    TObjectPtr<UGameDataEntry> SelectedEntry;

    /** Callbacks */
    FOnEntryAddedDelegate OnEntryAdded;
    FOnEntryDeletedDelegate OnEntryDeleted;
    FOnEntriesSwappedDelegate OnEntriesSwapped;
    FOnGetEntriesDelegate OnGetEntries;
    FOnEntriesModifiedDelegate OnEntriesModified;
  };
}
