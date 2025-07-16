#include "GameDataAssetActions.h"

#include "GameDataAssetEditor.h"

void FGameDataAssetActions::OpenAssetEditor(const TArray<UObject*>& InObjects,
                                            const TSharedPtr<IToolkitHost> EditWithinLevelEditor) {
  for (UObject* Object : InObjects) {
    if (const auto Asset = Cast<UGameDataAsset>(Object); Asset != nullptr) {
      const auto NewEditor = MakeShared<FGameDataAssetEditor>();
      NewEditor->Initialize(EToolkitMode::Standalone, EditWithinLevelEditor, Asset);
    }
  }
}
