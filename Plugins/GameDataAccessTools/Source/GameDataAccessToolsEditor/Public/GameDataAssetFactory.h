// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Factories/Factory.h"
#include "GameDataAssetFactory.generated.h"

/**
 *
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API UGameDataAssetFactory : public UFactory {
  GENERATED_BODY()

public:
  UGameDataAssetFactory();

  bool ConfigureProperties() override;

  UObject* FactoryCreateNew(UClass* InClass, UObject* InParent, FName InName, EObjectFlags Flags,
    UObject* Context, FFeedbackContext* Warn) override;

private:
  // Store the chosen class
  UPROPERTY()
  TObjectPtr<UClass> AssetClass;

};
