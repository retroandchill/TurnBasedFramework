// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "Subsystems/GameInstanceSubsystem.h"
#include "GameDataAccessSubsystem.generated.h"

namespace GameDataAccess {
  class FDataTableProxy;
}

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UGameDataAccessSubsystem : public UGameInstanceSubsystem {
  GENERATED_BODY()

public:
  void Initialize(FSubsystemCollectionBase& Collection) override;
  void Deinitialize() override;

private:
  TMap<FName, TSharedPtr<GameDataAccess::FDataTableProxy>> DataTableProxies;
};
