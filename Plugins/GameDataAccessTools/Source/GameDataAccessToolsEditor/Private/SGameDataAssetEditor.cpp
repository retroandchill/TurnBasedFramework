// Fill out your copyright notice in the Description page of Project Settings.


#include "SGameDataAssetEditor.h"

#include "SlateOptMacros.h"

namespace GameData {
  void SGameDataAssetEditor::Construct(const FArguments& InArgs) {
    DataAsset = InArgs._DataAsset;

  }
}
