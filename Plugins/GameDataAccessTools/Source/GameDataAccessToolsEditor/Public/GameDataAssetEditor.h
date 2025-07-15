// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Extensions/DataAssets/CSPrimaryDataAsset.h"

class SGameDataAssetEditor;
/**
 *
 */
class GAMEDATAACCESSTOOLSEDITOR_API FGameDataAssetEditor : public FAssetEditorToolkit {
public:
  void InitRpgDataAssetEditor(EToolkitMode::Type Mode, TSharedPtr<IToolkitHost> InitToolkitHost, UCSPrimaryDataAsset* AssetToEdit);

  FName GetToolkitFName() const override;
  FText GetBaseToolkitName() const override;
  FString GetWorldCentricTabPrefix() const override;
  FLinearColor GetWorldCentricTabColorScale() const override;

private:
  TSharedPtr<SGameDataAssetEditor> EditorWidget;
  TObjectPtr<UCSPrimaryDataAsset> EditedAsset;
};
