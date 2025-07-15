// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "Extensions/DataAssets/CSPrimaryDataAsset.h"
#include "GameDataAsset.generated.h"


class UGameDataEntry;
/**
 *
 */
UCLASS(abstract)
class GAMEDATAACCESSTOOLS_API UGameDataAsset : public UCSPrimaryDataAsset {
  GENERATED_BODY()

public:
  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  int32 GetNumEntries() const;

  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataAsset")
  UGameDataEntry* GetEntry(int32 Index) const;

};
