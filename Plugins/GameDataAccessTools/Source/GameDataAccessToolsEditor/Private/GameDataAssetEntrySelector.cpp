// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataAssetEntrySelector.h"

#include "SlateOptMacros.h"
#include "Widgets/Input/SSearchBox.h"


void SGameDataAssetEntrySelector::Construct(const FArguments& InArgs) {
  OnEntrySelected = InArgs._OnEntrySelected;
  OnGetEntries = InArgs._OnGetEntries;
  OnAddEntry = InArgs._OnAddEntry;
  OnDeleteEntry = InArgs._OnDeleteEntry;
  OnMoveEntryUp = InArgs._OnMoveEntryUp;
  OnMoveEntryDown = InArgs._OnMoveEntryDown;

  ChildSlot
    [
        SNew(SVerticalBox)

        // Search bar
        +SVerticalBox::Slot()
        .AutoHeight()
        .Padding(2)
        [
            SAssignNew(SearchBox, SSearchBox)
            .OnTextChanged(this, &SGameDataAssetEntrySelector::OnSearchTextChanged)
            .HintText(NSLOCTEXT("GameDataAssetEditor", "SearchBoxHint", "Search entries..."))
        ]

        // Toolbar with buttons
        +SVerticalBox::Slot()
        .AutoHeight()
        .Padding(2)
        [
            SNew(SHorizontalBox)

            // Add button
            +SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataAssetEntrySelector::AddEntryClicked)
                .Text(NSLOCTEXT("GameDataAssetEditor", "AddEntry", "Add Entry"))
            ]

            // Delete button
            +SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataAssetEntrySelector::DeleteEntryClicked)
                .IsEnabled_Raw(this, &SGameDataAssetEntrySelector::CanDeleteEntry)
                .Text(NSLOCTEXT("GameDataAssetEditor", "DeleteEntry", "Delete Entry"))
            ]

            // Move Up button
            +SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataAssetEntrySelector::MoveEntryUp)
                .IsEnabled_Raw(this, &SGameDataAssetEntrySelector::CanMoveEntryUp)
                .Text(NSLOCTEXT("GameDataAssetEditor", "MoveUp", "Move Up"))
            ]

            // Move Down button
            +SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataAssetEntrySelector::MoveEntryDown)
                .IsEnabled_Raw(this, &SGameDataAssetEntrySelector::CanMoveEntryDown)
                .Text(NSLOCTEXT("GameDataAssetEditor", "MoveDown", "Move Down"))
            ]
        ]

        // Entries list
        +SVerticalBox::Slot()
        .FillHeight(1.f)
        [
            SAssignNew(EntriesList, SListView<TSharedPtr<FEntryRowData>>)
            .ListItemsSource(&FilteredEntries)
            .OnGenerateRow(this, &SGameDataAssetEntrySelector::OnGenerateRow)
            .OnSelectionChanged(this, &SGameDataAssetEntrySelector::OnSelectionChanged)
            .SelectionMode(ESelectionMode::Single)
        ]
    ];

    RefreshList();
}

void SGameDataAssetEntrySelector::RefreshList() {
  if (OnGetEntries.IsBound())
  {
    AllEntries = OnGetEntries.Execute();
    FilteredEntries = AllEntries;
    EntriesList->RequestListRefresh();
  }

}

void SGameDataAssetEntrySelector::SelectAtIndex(const int32 Index) {
  if (Index >= 0 && Index < AllEntries.Num())
  {
    EntriesList->SetSelection(AllEntries[Index]);
  }
}

TSharedRef<ITableRow> SGameDataAssetEntrySelector::OnGenerateRow(TSharedPtr<FEntryRowData> Item,
                                                                 const TSharedRef<STableViewBase>& OwnerTable) {
  return SNew(STableRow<TSharedPtr<FEntryRowData>>, OwnerTable)
    [
        SNew(SHorizontalBox)

        // Index column
        +SHorizontalBox::Slot()
        .AutoWidth()
        .Padding(5)
        [
            SNew(STextBlock)
            .Text(FText::AsNumber(Item->Index))
        ]

        // Name column
        +SHorizontalBox::Slot()
        .FillWidth(1.0f)
        .Padding(5)
        [
            SNew(STextBlock)
            .Text(FText::FromName(Item->Name))
        ]
    ];

}

void SGameDataAssetEntrySelector::OnSearchTextChanged(const FText& InSearchText) {
  FilteredEntries.Empty();
  const FString SearchString = InSearchText.ToString();

  for (const auto& Entry : AllEntries)
  {
    if (SearchString.IsEmpty() ||
        Entry->Name.ToString().Contains(SearchString))
    {
      FilteredEntries.Add(Entry);
    }
  }

  EntriesList->RequestListRefresh();

}

void SGameDataAssetEntrySelector::OnSelectionChanged(TSharedPtr<FEntryRowData> Item, ESelectInfo::Type) const {
  if (OnEntrySelected.IsBound())
  {
    OnEntrySelected.Execute(Item);
  }
}

FReply SGameDataAssetEntrySelector::AddEntryClicked() const {
  if (OnAddEntry.IsBound())
  {
    OnAddEntry.Execute();
  }
  return FReply::Handled();

}

FReply SGameDataAssetEntrySelector::DeleteEntryClicked() const {
  if (OnDeleteEntry.IsBound())
  {
    OnDeleteEntry.Execute(EntriesList->GetSelectedItems()[0]);
  }
  return FReply::Handled();
}

FReply SGameDataAssetEntrySelector::MoveEntryUp() const {
  if (OnMoveEntryUp.IsBound())
  {
    OnMoveEntryUp.Execute(EntriesList->GetSelectedItems()[0]);
  }
  return FReply::Handled();

}

FReply SGameDataAssetEntrySelector::MoveEntryDown() const {
  if (OnMoveEntryDown.IsBound())
  {
    OnMoveEntryDown.Execute(EntriesList->GetSelectedItems()[0]);
  }
  return FReply::Handled();
}

bool SGameDataAssetEntrySelector::CanMoveEntryUp() const {
  return EntriesList->GetSelectedItems().Num() == 1 && AllEntries.Num() > 0 && EntriesList->GetSelectedItems()[0] != AllEntries[0];
}

bool SGameDataAssetEntrySelector::CanMoveEntryDown() const {
  return EntriesList->GetSelectedItems().Num() == 1 && AllEntries.Num() > 0 && EntriesList->GetSelectedItems().Last() != AllEntries.Last();
}

bool SGameDataAssetEntrySelector::CanDeleteEntry() const {

  return EntriesList->GetSelectedItems().Num() > 0;
}

