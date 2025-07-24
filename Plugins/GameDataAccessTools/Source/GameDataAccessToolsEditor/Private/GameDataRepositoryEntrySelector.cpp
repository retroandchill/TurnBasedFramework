// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataRepositoryEntrySelector.h"

#include "DesktopPlatformModule.h"
#include "IDesktopPlatform.h"
#include "SlateOptMacros.h"
#include "Interop/SerializationCallbacks.h"
#include "UnrealSharpProcHelper/CSProcHelper.h"
#include "Widgets/Input/SSearchBox.h"
#include "Widgets/Layout/SWrapBox.h"


void SGameDataRepositoryEntrySelector::Construct(const FArguments& InArgs)
{
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
        + SVerticalBox::Slot()
        .AutoHeight()
        .Padding(2)
        [
            SAssignNew(SearchBox, SSearchBox)
            .OnTextChanged(this, &SGameDataRepositoryEntrySelector::OnSearchTextChanged)
            .HintText(NSLOCTEXT("GameDataRepositoryEditor", "SearchBoxHint", "Search entries..."))
        ]

        // Toolbar with buttons
        + SVerticalBox::Slot()
        .AutoHeight()
        .Padding(2)
        [
            SNew(SHorizontalBox)

            // Add button
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataRepositoryEntrySelector::AddEntryClicked)
                .IsEnabled_Raw(this, &SGameDataRepositoryEntrySelector::CanAddEntry)
                .Text(NSLOCTEXT("GameDataRepositoryEditor", "AddEntry", "Add Entry"))
            ]

            // Delete button
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataRepositoryEntrySelector::DeleteEntryClicked)
                .IsEnabled_Raw(this, &SGameDataRepositoryEntrySelector::CanDeleteEntry)
                .Text(NSLOCTEXT("GameDataRepositoryEditor", "DeleteEntry", "Delete Entry"))
            ]

            // Move Up button
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataRepositoryEntrySelector::MoveEntryUp)
                .IsEnabled_Raw(this, &SGameDataRepositoryEntrySelector::CanMoveEntryUp)
                .Text(NSLOCTEXT("GameDataRepositoryEditor", "MoveUp", "Move Up"))
            ]

            // Move Down button
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(2)
            [
                SNew(SButton)
                .OnClicked(this, &SGameDataRepositoryEntrySelector::MoveEntryDown)
                .IsEnabled_Raw(this, &SGameDataRepositoryEntrySelector::CanMoveEntryDown)
                .Text(NSLOCTEXT("GameDataRepositoryEditor", "MoveDown", "Move Down"))
            ]
        ]
        // Entries list
        + SVerticalBox::Slot()
        .FillHeight(1.f)
        [
            SAssignNew(EntriesList, SListView<TSharedPtr<FEntryRowData>>)
            .ListItemsSource(&FilteredEntries)
            .OnGenerateRow(this, &SGameDataRepositoryEntrySelector::OnGenerateRow)
            .OnSelectionChanged(this, &SGameDataRepositoryEntrySelector::OnSelectionChanged)
            .SelectionMode(ESelectionMode::Single)
        ]
    ];

    RefreshList();
}

void SGameDataRepositoryEntrySelector::RefreshList()
{
    if (OnGetEntries.IsBound())
    {
        AllEntries = OnGetEntries.Execute();
        FilteredEntries = AllEntries;
        EntriesList->RequestListRefresh();
    }
}

void SGameDataRepositoryEntrySelector::SelectAtIndex(const int32 Index)
{
    if (Index >= 0 && Index < AllEntries.Num())
    {
        EntriesList->SetSelection(AllEntries[Index]);
    }
}

TSharedRef<ITableRow> SGameDataRepositoryEntrySelector::OnGenerateRow(TSharedPtr<FEntryRowData> Item,
                                                                      const TSharedRef<STableViewBase>& OwnerTable)
{
    return SNew(STableRow<TSharedPtr<FEntryRowData>>, OwnerTable)
        [
            SNew(SHorizontalBox)

            // Index column
            + SHorizontalBox::Slot()
            .AutoWidth()
            .Padding(5)
            [
                SNew(STextBlock)
                .Text(FText::AsNumber(Item->Index))
            ]

            // Name column
            + SHorizontalBox::Slot()
            .FillWidth(1.0f)
            .Padding(5)
            [
                SNew(STextBlock)
                .Text(FText::FromName(Item->Name))
            ]
        ];
}

void SGameDataRepositoryEntrySelector::OnSearchTextChanged(const FText& InSearchText)
{
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

void SGameDataRepositoryEntrySelector::OnSelectionChanged(TSharedPtr<FEntryRowData> Item, ESelectInfo::Type) const
{
    if (OnEntrySelected.IsBound())
    {
        OnEntrySelected.Execute(Item);
    }
}

FReply SGameDataRepositoryEntrySelector::AddEntryClicked() const
{
    if (OnAddEntry.IsBound())
    {
        OnAddEntry.Execute();
    }
    return FReply::Handled();
}

FReply SGameDataRepositoryEntrySelector::DeleteEntryClicked() const
{
    if (OnDeleteEntry.IsBound())
    {
        OnDeleteEntry.Execute(EntriesList->GetSelectedItems()[0]);
    }
    return FReply::Handled();
}

FReply SGameDataRepositoryEntrySelector::MoveEntryUp() const
{
    if (OnMoveEntryUp.IsBound())
    {
        OnMoveEntryUp.Execute(EntriesList->GetSelectedItems()[0]);
    }
    return FReply::Handled();
}

FReply SGameDataRepositoryEntrySelector::MoveEntryDown() const
{
    if (OnMoveEntryDown.IsBound())
    {
        OnMoveEntryDown.Execute(EntriesList->GetSelectedItems()[0]);
    }
    return FReply::Handled();
}

bool SGameDataRepositoryEntrySelector::CanAddEntry() const
{
    return EntriesList->GetSelectedItems().Num() < std::numeric_limits<int32>::max();
}

bool SGameDataRepositoryEntrySelector::CanMoveEntryUp() const
{
    return EntriesList->GetSelectedItems().Num() == 1 && AllEntries.Num() > 0 && EntriesList->GetSelectedItems()[0] !=
        AllEntries[0];
}

bool SGameDataRepositoryEntrySelector::CanMoveEntryDown() const
{
    return EntriesList->GetSelectedItems().Num() == 1 && AllEntries.Num() > 0 && EntriesList->GetSelectedItems().Last()
        != AllEntries.Last();
}

bool SGameDataRepositoryEntrySelector::CanDeleteEntry() const
{
    return EntriesList->GetSelectedItems().Num() > 0;
}
