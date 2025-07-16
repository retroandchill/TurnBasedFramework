// Fill out your copyright notice in the Description page of Project Settings.


#include "SGameDataAssetEditor.h"

#include "SlateOptMacros.h"
#include "Widgets/Layout/SSplitter.h"
#include "Widgets/Input/SButton.h"
#include "Widgets/Views/SListView.h"
#include "Widgets/Layout/SBox.h"
#include "Widgets/Layout/SScrollBox.h"
#include "Widgets/Text/STextBlock.h"
#include "PropertyEditorModule.h"
#include "Modules/ModuleManager.h"
#include "DetailLayoutBuilder.h"
#include "DataRetrieval/GameDataEntry.h"

BEGIN_SLATE_FUNCTION_BUILD_OPTIMIZATION

namespace GameData {
  class SEntryRow : public STableRow<TObjectPtr<UGameDataEntry>> {
  public:
    SLATE_BEGIN_ARGS(SEntryRow) {}
    SLATE_END_ARGS()

    void Construct(const FArguments& InArgs, const TSharedRef<STableViewBase>& OwnerTable, TObjectPtr<UGameDataEntry> InEntry) {
      Entry = InEntry;

      STableRow::Construct(
        STableRow::FArguments()
        .Style(FAppStyle::Get(), "TableView.Row")
        .Padding(FMargin(4.0f, 2.0f)),
        OwnerTable
      );

      ChildSlot
      [
        SNew(SHorizontalBox)
        + SHorizontalBox::Slot()
        .AutoWidth()
        .VAlign(VAlign_Center)
        .Padding(4.0f, 0.0f)
        [
          SNew(STextBlock)
          .Text(FText::FromString(FString::Printf(TEXT("%d"), Entry->GetRowIndex())))
        ]
        + SHorizontalBox::Slot()
        .FillWidth(1.0f)
        .VAlign(VAlign_Center)
        .Padding(4.0f, 0.0f)
        [
          SNew(STextBlock)
          .Text(FText::FromName(Entry->GetId()))
        ]
      ];
    }

  private:
    TObjectPtr<UGameDataEntry> Entry;
  };

  void SGameDataAssetEditor::Construct(const FArguments& InArgs) {
    DataAsset = InArgs._DataAsset;
    OnEntryAdded = InArgs._OnEntryAdded;
    OnEntryDeleted = InArgs._OnEntryDeleted;
    OnEntriesSwapped = InArgs._OnEntriesSwapped;
    OnGetEntries = InArgs._OnGetEntries;
    OnEntriesModified = InArgs._OnEntriesModified;

    // Create entry list view
    EntryListView = SNew(SListView<TObjectPtr<UGameDataEntry>>)
      .ListItemsSource(&Entries)
      .OnGenerateRow(this, &SGameDataAssetEditor::OnGenerateEntryRow)
      .OnSelectionChanged(this, &SGameDataAssetEditor::OnEntrySelectionChanged)
      .SelectionMode(ESelectionMode::Single);

    // Create the property editor
    FPropertyEditorModule& PropertyEditorModule = FModuleManager::GetModuleChecked<FPropertyEditorModule>("PropertyEditor");
    FDetailsViewArgs DetailsViewArgs;
    DetailsViewArgs.bUpdatesFromSelection = false;
    DetailsViewArgs.bLockable = false;
    DetailsViewArgs.bAllowSearch = true;
    DetailsViewArgs.NameAreaSettings = FDetailsViewArgs::HideNameArea;
    DetailsViewArgs.bHideSelectionTip = true;
    DetailsViewArgs.NotifyHook = nullptr;
    DetailsViewArgs.bSearchInitialKeyFocus = false;
    DetailsViewArgs.ViewIdentifier = NAME_None;
    DetailsViewArgs.DefaultsOnlyVisibility = EEditDefaultsOnlyNodeVisibility::Automatic;
    DetailsViewArgs.bShowOptions = true;
    DetailsViewArgs.bAllowMultipleTopLevelObjects = true;

    EntryDetailsView = PropertyEditorModule.CreateDetailView(DetailsViewArgs);

    // Construct the main widget
    ChildSlot
    [
      SNew(SVerticalBox)
      + SVerticalBox::Slot()
      .FillHeight(1.0f)
      [
        SNew(SSplitter)
        .Orientation(Orient_Horizontal)
        + SSplitter::Slot()
        .Value(0.3f)
        [
          SNew(SVerticalBox)
          + SVerticalBox::Slot()
          .AutoHeight()
          .Padding(2.0f)
          [
            SNew(SHorizontalBox)
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2.0f)
            [
              SNew(SButton)
              .Text(FText::FromString(TEXT("Add")))
              .ToolTipText(FText::FromString(TEXT("Add a new entry")))
              .OnClicked(this, &SGameDataAssetEditor::OnAddEntryClicked)
            ]
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2.0f)
            [
              SNew(SButton)
              .Text(FText::FromString(TEXT("Delete")))
              .ToolTipText(FText::FromString(TEXT("Delete the selected entry")))
              .OnClicked(this, &SGameDataAssetEditor::OnDeleteEntryClicked)
              .IsEnabled_Lambda([this]() -> bool { return SelectedEntry != nullptr; })
            ]
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2.0f)
            [
              SNew(SButton)
              .Text(FText::FromString(TEXT("Move Up")))
              .ToolTipText(FText::FromString(TEXT("Move the selected entry up")))
              .OnClicked(this, &SGameDataAssetEditor::OnMoveEntryUpClicked)
              .IsEnabled_Lambda([this]() -> bool {
                return SelectedEntry != nullptr && Entries.Find(SelectedEntry) > 0;
              })
            ]
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2.0f)
            [
              SNew(SButton)
              .Text(FText::FromString(TEXT("Move Down")))
              .ToolTipText(FText::FromString(TEXT("Move the selected entry down")))
              .OnClicked(this, &SGameDataAssetEditor::OnMoveEntryDownClicked)
              .IsEnabled_Lambda([this]() -> bool {
                return SelectedEntry != nullptr &&
                       Entries.Find(SelectedEntry) < Entries.Num() - 1;
              })
            ]
          ]
          + SVerticalBox::Slot()
          .FillHeight(1.0f)
          [
            SNew(SBorder)
            .BorderImage(FAppStyle::GetBrush("ToolPanel.GroupBorder"))
            .Padding(FMargin(4.0f))
            [
              SNew(SScrollBox)
              + SScrollBox::Slot()
              [
                EntryListView.ToSharedRef()
              ]
            ]
          ]
        ]
        + SSplitter::Slot()
        .Value(0.7f)
        [
          SNew(SBorder)
          .BorderImage(FAppStyle::GetBrush("ToolPanel.GroupBorder"))
          .Padding(FMargin(4.0f))
          [
            EntryDetailsView.ToSharedRef()
          ]
        ]
      ]
    ];

    // Refresh the entry list
    RefreshEntryList();
  }

  void SGameDataAssetEditor::RefreshEntryList() {
    if (OnGetEntries.IsBound()) {
      Entries = OnGetEntries.Execute();
      EntryListView->RequestListRefresh();
    }
  }

  TSharedRef<ITableRow> SGameDataAssetEditor::OnGenerateEntryRow(TObjectPtr<UGameDataEntry> InEntry, const TSharedRef<STableViewBase>& OwnerTable) {
    return SNew(SEntryRow, OwnerTable, InEntry);
  }

  void SGameDataAssetEditor::OnEntrySelectionChanged(TObjectPtr<UGameDataEntry> InEntry, ESelectInfo::Type SelectInfo) {
    SelectedEntry = InEntry;

    if (EntryDetailsView.IsValid()) {
      EntryDetailsView->SetObject(InEntry);
    }
  }

  FReply SGameDataAssetEditor::OnAddEntryClicked() {
    if (OnEntryAdded.IsBound()) {
      // The actual entry creation is handled by the callback
      UGameDataEntry* NewEntry = nullptr;
      OnEntryAdded.ExecuteIfBound(NewEntry);

      // Refresh the list after addition
      RefreshEntryList();

      // Select the newly created entry
      if (NewEntry) {
        EntryListView->SetSelection(NewEntry);
      }
    }

    return FReply::Handled();
  }

  FReply SGameDataAssetEditor::OnDeleteEntryClicked() {
    if (SelectedEntry && OnEntryDeleted.IsBound()) {
      int32 CurrentIndex = Entries.Find(SelectedEntry);
      OnEntryDeleted.ExecuteIfBound(CurrentIndex);

      // Refresh the list after deletion
      RefreshEntryList();

      // Clear selection
      SelectedEntry = nullptr;
      EntryDetailsView->SetObject(nullptr);
    }

    return FReply::Handled();
  }

  FReply SGameDataAssetEditor::OnMoveEntryUpClicked() {
    if (SelectedEntry && OnEntriesSwapped.IsBound()) {
      int32 CurrentIndex = Entries.Find(SelectedEntry);

      if (CurrentIndex > 0) {
        OnEntriesSwapped.ExecuteIfBound(CurrentIndex, CurrentIndex - 1);

        // Refresh the list after swapping
        RefreshEntryList();

        // Maintain selection
        EntryListView->SetSelection(SelectedEntry);
      }
    }

    return FReply::Handled();
  }

  FReply SGameDataAssetEditor::OnMoveEntryDownClicked() {
    if (SelectedEntry && OnEntriesSwapped.IsBound()) {
      if (int32 CurrentIndex = Entries.Find(SelectedEntry); CurrentIndex < Entries.Num() - 1) {
        OnEntriesSwapped.ExecuteIfBound(CurrentIndex, CurrentIndex + 1);

        // Refresh the list after swapping
        RefreshEntryList();

        // Maintain selection
        EntryListView->SetSelection(SelectedEntry);
      }
    }

    return FReply::Handled();
  }
}

END_SLATE_FUNCTION_BUILD_OPTIMIZATION
