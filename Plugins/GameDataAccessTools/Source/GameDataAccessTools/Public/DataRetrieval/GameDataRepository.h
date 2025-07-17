// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "Extensions/DataAssets/CSPrimaryDataAsset.h"
#include "GameDataRepository.generated.h"

class UGameDataEntry;
/**
 *
 */
UCLASS(abstract)
class GAMEDATAACCESSTOOLS_API UGameDataRepository : public UObject {
  GENERATED_BODY()

public:
  UFUNCTION(BlueprintImplementableEvent, Category = "GameDataRepository")
  TSubclassOf<UGameDataEntry> GetEntryClass() const;

};
