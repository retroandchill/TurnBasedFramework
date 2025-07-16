// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Widgets/SCompoundWidget.h"

class UCSPrimaryDataAsset;

namespace GameData {
  /**
   *
   */
  class GAMEDATAACCESSTOOLSEDITOR_API SGameDataAssetEditor : public SCompoundWidget {
  public:
    SLATE_BEGIN_ARGS(SGameDataAssetEditor) {
    }

    SLATE_ARGUMENT(UCSPrimaryDataAsset*, DataAsset)
  SLATE_END_ARGS()

  void Construct(const FArguments& InArgs);

  private:
    TObjectPtr<UCSPrimaryDataAsset> DataAsset;

  };
}
