#pragma once

#include "AssetTypeActions_Base.h"
#include "DataRetrieval/GameDataAsset.h"

class FGameDataAssetActions final : public FAssetTypeActions_Base {
public:
  // IAssetTypeActions Implementation
  FText GetName() const override { return NSLOCTEXT("AssetTypeActions", "AssetTypeActions_YourCustomAsset", "Your Custom Asset"); }
  FColor GetTypeColor() const override { return FColor(140, 80, 200); }
  UClass* GetSupportedClass() const override { return UGameDataAsset::StaticClass(); }
  uint32 GetCategories() override { return EAssetTypeCategories::Misc; }

  // Custom editor implementation
  void OpenAssetEditor(const TArray<UObject*>& InObjects, const TSharedPtr<IToolkitHost> EditWithinLevelEditor) override;

};
