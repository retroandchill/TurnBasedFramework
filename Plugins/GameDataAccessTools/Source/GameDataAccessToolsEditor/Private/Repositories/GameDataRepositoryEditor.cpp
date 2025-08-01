﻿// Fill out your copyright notice in the Description page of Project Settings.


#include "Repositories/GameDataRepositoryEditor.h"

#include <bit>

#include "SGameplayTagWidget.h"
#include "DesktopPlatformModule.h"
#include "EditorDirectories.h"
#include "SGameplayTagPicker.h"
#include "Repositories/GameDataRepositoryEntrySelector.h"
#include "DataRetrieval/GameDataRepository.h"
#include "Interop/GameDataEntrySerializer.h"
#include "Interop/SerializationCallbacks.h"


class IDesktopPlatform;

void FGameDataRepositoryEditor::Initialize(const EToolkitMode::Type Mode,
                                           const TSharedPtr<IToolkitHost>& InitToolkitHost,
                                           UGameDataRepository* Asset)
{
    GameDataRepository = Asset;

    const auto AssetClass = Asset->GetClass();
    const auto DataEntriesProperty = CastFieldChecked<FArrayProperty>(
        AssetClass->FindPropertyByName(TEXT("DataEntries")));
    GameDataEntries = std::bit_cast<TArray<UObject*>*>(
        DataEntriesProperty->GetPropertyValuePtr_InContainer(Asset));

    const auto EntryClass = Asset->GetEntryClass();
    IdProperty = CastFieldChecked<FStructProperty>(EntryClass->FindPropertyByName(TEXT("Id")));
    RowIndexProperty = CastFieldChecked<FIntProperty>(EntryClass->FindPropertyByName(TEXT("RowIndex")));

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

    Serializers = FSerializationCallbacks::Get().GetSerializationActions(Asset->GetEntryClass());

    const auto ToolbarExtender = MakeShared<FExtender>();
    ToolbarExtender->AddToolBarExtension(
        "Asset",
        EExtensionHook::After,
        GetToolkitCommands(),
        FToolBarExtensionDelegate::CreateRaw(this, &FGameDataRepositoryEditor::FillToolbar)
    );
    AddToolbarExtender(ToolbarExtender);

    InitAssetEditor(Mode, InitToolkitHost, "GameDataRepositoryEditor", Layout, true, true, Asset);
}

void FGameDataRepositoryEditor::RegisterTabSpawners(const TSharedRef<FTabManager>& InTabManager)
{
    FAssetEditorToolkit::RegisterTabSpawners(InTabManager);

    WorkspaceMenuCategory = InTabManager->AddLocalWorkspaceMenuCategory(
        NSLOCTEXT("GameDataRepository", "GameDataRepository", "Game Data Asset"));

    InTabManager->RegisterTabSpawner("EntrySelectionTab", FOnSpawnTab::CreateLambda([this](const FSpawnTabArgs&)
                {
                    return SNew(SDockTab)
                        [
                            SAssignNew(EntrySelector, SGameDataRepositoryEntrySelector)
                            .OnEntrySelected(this, &FGameDataRepositoryEditor::OnEntrySelected)
                            .OnGetEntries(this, &FGameDataRepositoryEditor::OnGetEntries)
                        ];
                }))
                .SetDisplayName(NSLOCTEXT("GameDataRepository", "EntrySelectionTab", "Entries"))
                .SetGroup(WorkspaceMenuCategory.ToSharedRef());

    FPropertyEditorModule& PropertyEditorModule = FModuleManager::GetModuleChecked<FPropertyEditorModule>(
        "PropertyEditor");
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

void FGameDataRepositoryEditor::UnregisterTabSpawners(const TSharedRef<FTabManager>& InTabManager)
{
    FAssetEditorToolkit::UnregisterTabSpawners(InTabManager);
    InTabManager->UnregisterTabSpawner("EntrySelectionTab");
    InTabManager->UnregisterTabSpawner("EntryEditTab");
}

FName FGameDataRepositoryEditor::GetToolkitFName() const
{
    return FName("GameDataRepositoryEditor");
}

FText FGameDataRepositoryEditor::GetBaseToolkitName() const
{
    return NSLOCTEXT("GameDataRepositoryEditor", "AppLabel", "Game Data Asset Editor");
}

FString FGameDataRepositoryEditor::GetWorldCentricTabPrefix() const
{
    return "GameDataRepositoryEditor";
}

FLinearColor FGameDataRepositoryEditor::GetWorldCentricTabColorScale() const
{
    return FLinearColor();
}

void FGameDataRepositoryEditor::FillToolbar(FToolBarBuilder& ToolbarBuilder)
{
    // Add import section
    ToolbarBuilder.BeginSection("Import");
    {
        // Add import button that will be populated with dynamic actions
        ToolbarBuilder.AddComboButton(
            FUIAction(),
            FOnGetContent::CreateSP(this, &FGameDataRepositoryEditor::ImportMenuEntries),
            NSLOCTEXT("GameDataRepository", "Import", "Import"),
            NSLOCTEXT("GameDataRepository", "ImportTooltip", "Import data from various sources"),
            FSlateIcon(FAppStyle::GetAppStyleSetName(), "Icons.Reimport")
        );
    }
    ToolbarBuilder.EndSection();

    // Add entry management section
    ToolbarBuilder.BeginSection("EntryManagement");
    {
        ToolbarBuilder.AddToolBarButton(
            FUIAction(
                FExecuteAction::CreateSP(this, &FGameDataRepositoryEditor::OnAddEntry),
                FCanExecuteAction::CreateRaw(this, &FGameDataRepositoryEditor::CanAddEntry)
            ),
            NAME_None,
            NSLOCTEXT("GameDataRepository", "AddEntry", "Add"),
            NSLOCTEXT("GameDataRepository", "AddEntryTooltip", "Add new entry"),
            FSlateIcon(FAppStyle::GetAppStyleSetName(), "Plus")
        );

        ToolbarBuilder.AddToolBarButton(
            FUIAction(
                FExecuteAction::CreateSP(this, &FGameDataRepositoryEditor::OnDeleteEntry),
                FCanExecuteAction::CreateRaw(this, &FGameDataRepositoryEditor::CanDeleteEntry)
            ),
            NAME_None,
            NSLOCTEXT("GameDataRepository", "DeleteEntry", "Delete"),
            NSLOCTEXT("GameDataRepository", "DeleteEntryTooltip", "Delete selected entry"),
            FSlateIcon(FAppStyle::GetAppStyleSetName(), "Cross")
        );

        ToolbarBuilder.AddToolBarButton(
            FUIAction(
                FExecuteAction::CreateSP(this, &FGameDataRepositoryEditor::OnMoveEntryUp),
                FCanExecuteAction::CreateRaw(this, &FGameDataRepositoryEditor::CanMoveEntryUp)
            ),
            NAME_None,
            NSLOCTEXT("GameDataRepository", "MoveUp", "Move Up"),
            NSLOCTEXT("GameDataRepository", "MoveUpTooltip", "Move selected entry up"),
            FSlateIcon(FAppStyle::GetAppStyleSetName(), "ArrowUp")
        );

        ToolbarBuilder.AddToolBarButton(
            FUIAction(
                FExecuteAction::CreateSP(this, &FGameDataRepositoryEditor::OnMoveEntryDown),
                FCanExecuteAction::CreateRaw(this, &FGameDataRepositoryEditor::CanMoveEntryDown)
            ),
            NAME_None,
            NSLOCTEXT("GameDataRepository", "MoveDown", "Move Down"),
            NSLOCTEXT("GameDataRepository", "MoveDownTooltip", "Move selected entry down"),
            FSlateIcon(FAppStyle::GetAppStyleSetName(), "ArrowDown")
        );
    }
    ToolbarBuilder.EndSection();
}

TSharedRef<SWidget> FGameDataRepositoryEditor::ImportMenuEntries()
{
    FMenuBuilder MenuBuilder(true, GetToolkitCommands());

    for (const auto& Serializer : Serializers)
    {
        const auto FormatName = Serializer->GetFormatName();
        // Add your dynamic import actions here
        MenuBuilder.AddMenuEntry(
            FText::Format(NSLOCTEXT("GameDataRepository", "ImportFile", "Import from {0}"), FormatName),
            FText::Format(
                NSLOCTEXT("GameDataRepository", "ImportTooltip", "Import entries from a {0} file"), FormatName),
            FSlateIcon(),
            FUIAction(FExecuteAction::CreateSP(this, &FGameDataRepositoryEditor::ImportGameDataRepository, Serializer))
        );
    }


    return MenuBuilder.MakeWidget();
}

void FGameDataRepositoryEditor::ImportGameDataRepository(TSharedRef<FGameDataEntrySerializer> Serializer)
{
    if (IDesktopPlatform* DesktopPlatform = FDesktopPlatformModule::Get(); DesktopPlatform != nullptr)
    {
        const void* ParentWindowWindowHandle = FSlateApplication::Get().FindBestParentWindowHandleForDialogs(nullptr);
        TArray<FString> FileNames;
        if (!DesktopPlatform->OpenFileDialog(
            ParentWindowWindowHandle,
            NSLOCTEXT("GameDataRepository", "Select file to import from", "Select file to import from...").ToString(),
            *FEditorDirectories::Get().GetLastDirectory(ELastDirectory::UNR),
            TEXT(""),
            Serializer->GetFileExtensionText(),
            EFileDialogFlags::None,
            FileNames))
        {
            return;
        }

        const FString& FileName = FileNames[0];
        FString FileContent;
        if (!FFileHelper::LoadFileToString(FileContent, *FileName))
        {
            FMessageDialog::Open(EAppMsgType::Ok, NSLOCTEXT("GameDataRepository", "OpenFailed", "Failed to open file"));
            return;
        }
        FEditorDirectories::Get().SetLastDirectory(ELastDirectory::UNR, *FPaths::GetPath(FileName));
        if (auto Serialized = Serializer->DeserializeData(FileContent, GameDataRepository); Serialized.has_value())
        {
            *GameDataEntries = MoveTemp(Serialized.value());
            RefreshList();
            FMessageDialog::Open(EAppMsgType::Ok, NSLOCTEXT("GameDataRepository", "ImportSuccessful", "Import was successful!"));
        }
        else
        {
            FMessageDialog::Open(EAppMsgType::Ok, FText::FromString(MoveTemp(Serialized.error())));
        }
    }
}

bool FGameDataRepositoryEditor::CanAddEntry() const
{
    return EntrySelector->GetSelectedEntries().Num() < std::numeric_limits<int32>::max();
}

bool FGameDataRepositoryEditor::CanMoveEntryUp() const
{
    if (EntrySelector->IsFiltering())
    {
        return false;
    }

    auto SelectedItems = EntrySelector->GetSelectedEntries();
    auto& AllEntries = EntrySelector->GetEntries();
    return SelectedItems.Num() == 1 && AllEntries.Num() > 0 && SelectedItems[0] != AllEntries[0];
}

bool FGameDataRepositoryEditor::CanMoveEntryDown() const
{
    if (EntrySelector->IsFiltering())
    {
        return false;
    }

    auto SelectedItems = EntrySelector->GetSelectedEntries();
    auto& AllEntries = EntrySelector->GetEntries();
    return SelectedItems.Num() == 1 && AllEntries.Num() > 0 && SelectedItems.Last() != AllEntries.Last();
}

bool FGameDataRepositoryEditor::CanDeleteEntry() const
{
    return EntrySelector->GetSelectedEntries().Num() > 0;
}

void FGameDataRepositoryEditor::OnEntrySelected(const TSharedPtr<FEntryRowData>& Entry)
{
    if (Entry != nullptr)
    {
        DetailsView->SetObject(Entry->Entry.Get());
        CurrentRow.Emplace(Entry->Index, Entry->Id);
    }
    else
    {
        DetailsView->SetObject(nullptr);
        CurrentRow.Reset();
    }
}

TArray<TSharedPtr<FEntryRowData>> FGameDataRepositoryEditor::OnGetEntries() const
{
    TArray<TSharedPtr<FEntryRowData>> Entries;
    for (int32 i = 0; i < GameDataEntries->Num(); i++)
    {
        auto Entry = (*GameDataEntries)[i];
        Entries.Emplace(MakeShared<FEntryRowData>(i, GetId(Entry), Entry));
    }
    return Entries;
}

void FGameDataRepositoryEditor::OnAddEntry() const
{
    auto RowName = GenerateUniqueRowName();
    if (!RowName.IsSet()) return;
    
    const auto NewEntry = NewObject<UObject>(GameDataRepository, GameDataRepository->GetEntryClass());
    SetId(NewEntry, *RowName);
    SetRowIndex(NewEntry, GameDataEntries->Num());
    GameDataEntries->Add(NewEntry);
    RefreshList();
}

void FGameDataRepositoryEditor::OnDeleteEntry()
{
    check(CurrentRow.IsSet());
    const auto& Entry = CurrentRow.GetValue();
    GameDataEntries->RemoveAt(Entry.Index);
    if (GameDataEntries->Num() == 0)
    {
        CurrentRow.Reset();
    }
    else if (Entry.Index <= GameDataEntries->Num())
    {
        int32 NewIndex = GameDataEntries->Num() - 1;

        CurrentRow.Emplace(NewIndex, GetId((*GameDataEntries)[NewIndex]));
    }
    RefreshList();
}

void FGameDataRepositoryEditor::OnMoveEntryUp()
{
    check(CurrentRow.IsSet());
    const auto& Entry = CurrentRow.GetValue();
    GameDataEntries->Swap(Entry.Index, Entry.Index - 1);
    CurrentRow.Emplace(Entry.Index - 1, GetId((*GameDataEntries)[Entry.Index - 1]));
    RefreshList();
}

void FGameDataRepositoryEditor::OnMoveEntryDown()
{
    check(CurrentRow.IsSet());
    const auto& Entry = CurrentRow.GetValue();
    GameDataEntries->Swap(Entry.Index, Entry.Index + 1);
    CurrentRow.Emplace(Entry.Index + 1, GetId((*GameDataEntries)[Entry.Index + 1]));
    RefreshList();
}

void FGameDataRepositoryEditor::RefreshList() const
{
    for (int32 i = 0; i < GameDataEntries->Num(); i++)
    {
        const auto Entry = (*GameDataEntries)[i];
        SetRowIndex(Entry, i);
    }
    GameDataRepository->Refresh();
    GameDataRepository->Modify();
    EntrySelector->RefreshList();
    if (CurrentRow.IsSet())
    {
        EntrySelector->SelectAtIndex(CurrentRow.GetValue().Index);
    }
}

TOptional<FGameplayTag> FGameDataRepositoryEditor::GenerateUniqueRowName() const
{
    TSet<FGameplayTag> UsedNames;
    for (const auto& Entry : *GameDataEntries)
    {
        UsedNames.Add(GetId(Entry));
    }

    const auto Prefix = IdProperty->GetMetaData("Categories");
    FString SuggestedName = Prefix.IsEmpty() ? TEXT("Entry") : Prefix;
    
    // Show dialog to get user input
    auto Window = SNew(SWindow)
        .Title(FText::FromString(TEXT("Select a unique gameplay tag")))
        .ClientSize(FVector2D(400, 100))
        .SizingRule(ESizingRule::Autosized);

    bool bConfirmed = false;
    TOptional<FGameplayTag> CurrentSelection;
    
    Window->SetContent(
        SNew(SVerticalBox)
        + SVerticalBox::Slot()
        .Padding(10)
        [
            SNew(SGameplayTagPicker)
                .MultiSelect(false)
                .Filter(Prefix)
                .GameplayTagPickerMode(EGameplayTagPickerMode::HybridMode)
                .TagContainers({ FGameplayTagContainer() })
                .OnTagChanged(SGameplayTagPicker::FOnTagChanged::CreateLambda([&CurrentSelection, &UsedNames](const TArray<FGameplayTagContainer>& Tags) {
                    if (Tags.Num() > 0 && !UsedNames.Contains(Tags[0].First()))
                    {
                        CurrentSelection.Emplace(Tags[0].First());
                    }
                    else
                    {
                        CurrentSelection.Reset();
                    }
                }))
        ]
        + SVerticalBox::Slot()
        .Padding(10)
        .AutoHeight()
        .HAlign(HAlign_Right)
        [
            SNew(SButton)
                .Text(FText::FromString(TEXT("OK")))
                .IsEnabled_Lambda([&CurrentSelection]
                {
                    return CurrentSelection.IsSet();
                })
            .OnClicked_Lambda([&bConfirmed, &Window]
            {
                bConfirmed = true;
                Window->RequestDestroyWindow();
                return FReply::Handled();
            })
        ]
    );

    FSlateApplication::Get().AddModalWindow(Window, FGlobalTabmanager::Get()->GetRootWindow());

    
    return bConfirmed ? CurrentSelection : TOptional<FGameplayTag>();
}

bool FGameDataRepositoryEditor::VerifyRowNameUnique(const FGameplayTag Name) const
{
    bool bFirstInstance = false;
    for (const auto& Entry : *GameDataEntries)
    {
        if (GetId(Entry) == Name)
        {
            if (!bFirstInstance)
            {
                bFirstInstance = true;
                continue;
            }
            return false;
        }
    }

    return true;
}

void FGameDataRepositoryEditor::OnPropertyChanged(const FPropertyChangedEvent& PropertyChangedEvent)
{
    if (PropertyChangedEvent.GetPropertyName() != "Id")
    {
        return;
    }

    if (!CurrentRow.IsSet())
    {
        return;
    }
    auto& [Index, CurrentName] = CurrentRow.GetValue();
    const auto SelectedEntry = (*GameDataEntries)[Index];
    if (auto Id = GetId(SelectedEntry); !Id.IsValid() || !VerifyRowNameUnique(Id))
    {
        SetId(SelectedEntry, CurrentName);
    }
    RefreshList();
}

FGameplayTag FGameDataRepositoryEditor::GetId(const UObject* Entry) const
{
    FGameplayTag Result;
    IdProperty->GetValue_InContainer(Entry, &Result);
    return Result;
}

void FGameDataRepositoryEditor::SetId(UObject* Entry, const FGameplayTag Id) const
{
    IdProperty->SetValue_InContainer(Entry, &Id);
}

int32 FGameDataRepositoryEditor::GetRowIndex(const UObject* Entry) const
{
    return RowIndexProperty->GetPropertyValue_InContainer(Entry);
}

void FGameDataRepositoryEditor::SetRowIndex(UObject* Entry, const int32 Id) const
{
    RowIndexProperty->SetPropertyValue_InContainer(Entry, Id);   
}
