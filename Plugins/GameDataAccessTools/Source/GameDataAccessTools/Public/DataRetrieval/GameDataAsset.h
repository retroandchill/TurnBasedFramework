// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "Extensions/DataAssets/CSPrimaryDataAsset.h"
#include "GameDataAsset.generated.h"

#if WITH_EDITORONLY_DATA
namespace GameData {
  class FGameDataAssetEditor;
}
#endif

class UGameDataEntry;
/**
 *
 */
UCLASS(abstract)
class GAMEDATAACCESSTOOLS_API UGameDataAsset : public UCSPrimaryDataAsset {
  GENERATED_BODY()

public:
  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  TSubclassOf<UGameDataEntry> GetEntryClass() const;

  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  int32 GetNumEntries() const;

  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  UGameDataEntry* GetEntry(int32 Index) const;

protected:
#if WITH_EDITORONLY_DATA
  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  void AddEntry(UGameDataEntry* NewEntry);

  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  void RemoveEntry(int32 Index);

  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  void SwapEntries(int32 Index1, int32 Index2);

  friend GameData::FGameDataAssetEditor;
#endif

};
