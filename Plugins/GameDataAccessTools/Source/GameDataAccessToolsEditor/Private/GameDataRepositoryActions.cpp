#include "GameDataRepositoryActions.h"

#include "GameDataRepositoryEditor.h"
#include "DataRetrieval/GameDataRepository.h"

FText FGameDataRepositoryActions::GetName() const
{
    return NSLOCTEXT("AssetTypeActions", "AssetTypeActions_GameDataRepository", "Game Data Repository");
}

FColor FGameDataRepositoryActions::GetTypeColor() const
{
    return FColor(140, 80, 200);
}

UClass* FGameDataRepositoryActions::GetSupportedClass() const
{
    return UGameDataRepository::StaticClass();
}

uint32 FGameDataRepositoryActions::GetCategories()
{
    return EAssetTypeCategories::Misc;
}

void FGameDataRepositoryActions::OpenAssetEditor(const TArray<UObject*>& InObjects,
                                                 const TSharedPtr<IToolkitHost> EditWithinLevelEditor)
{
    for (UObject* Object : InObjects)
    {
        if (const auto Asset = Cast<UGameDataRepository>(Object); Asset != nullptr)
        {
            const auto NewEditor = MakeShared<FGameDataRepositoryEditor>();
            NewEditor->Initialize(EToolkitMode::Standalone, EditWithinLevelEditor, Asset);
        }
    }
}
