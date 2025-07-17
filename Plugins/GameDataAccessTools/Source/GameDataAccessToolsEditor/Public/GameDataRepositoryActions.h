#pragma once

#include "AssetTypeActions_Base.h"

class FGameDataRepositoryActions final : public FAssetTypeActions_Base {
public:
  FText GetName() const override;
  FColor GetTypeColor() const override;
  UClass* GetSupportedClass() const override;
  uint32 GetCategories() override;

  void OpenAssetEditor(const TArray<UObject*>& InObjects, const TSharedPtr<IToolkitHost> EditWithinLevelEditor) override;

};
