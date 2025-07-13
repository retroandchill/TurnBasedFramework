// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "Engine/DeveloperSettings.h"
#include "GameDataAccessSettings.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UGameDataAccessSettings : public UDeveloperSettings {
  GENERATED_BODY()

public:

private:
  UPROPERTY(EditDefaultsOnly, Category = "DataAccess")
  TMap<FGameplayTag, FSoftObjectPath> StaticRegistrations;
};
