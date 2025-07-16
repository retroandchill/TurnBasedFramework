// Fill out your copyright notice in the Description page of Project Settings.


#include "GameDataAssetFactory.h"

#include "ClassViewerFilter.h"
#include "ClassViewerModule.h"
#include "DataRetrieval/GameDataEntry.h"
#include "DataRetrieval/GameDataAsset.h"
#include "Kismet2/SClassPickerDialog.h"

class FGameDataAssetFilter final : public IClassViewerFilter {
public:
  bool IsClassAllowed(const FClassViewerInitializationOptions& InInitOptions, const UClass* InClass,
                      const TSharedRef<FClassViewerFilterFuncs> InFilterFuncs) override {
    if (!InClass->IsChildOf<UGameDataAsset>() || InClass->HasAnyClassFlags(CLASS_Abstract)) {
      return false;
    }

    const auto DataEntriesProperty = CastField<FArrayProperty>(InClass->FindPropertyByName(TEXT("DataEntries")));
    if (DataEntriesProperty == nullptr) {
      return false;
    }

    const auto InnerProperty = CastField<FObjectProperty>(DataEntriesProperty->Inner);
    return InnerProperty != nullptr && InnerProperty->PropertyClass->IsChildOf<UGameDataEntry>();
  }

  bool IsUnloadedClassAllowed(const FClassViewerInitializationOptions& InInitOptions,
                              const TSharedRef<const IUnloadedBlueprintData> InUnloadedClassData,
                              const TSharedRef<FClassViewerFilterFuncs> InFilterFuncs) override {
    return false;
  }
};

UGameDataAssetFactory::UGameDataAssetFactory() {
  bCreateNew = true;
  bEditAfterNew = true;
  SupportedClass = UGameDataAsset::StaticClass();
}

bool UGameDataAssetFactory::ConfigureProperties() {
  // Configure the class viewer
  FClassViewerInitializationOptions Options;
  Options.Mode = EClassViewerMode::ClassPicker;
  Options.DisplayMode = EClassViewerDisplayMode::DefaultView;
  Options.bShowObjectRootClass = false;

  // Only show classes that are non-abstract and inherit from our base class
  const auto Filter = MakeShared<FGameDataAssetFilter>();
  Options.ClassFilters.Add(Filter);

  const FText TitleText = NSLOCTEXT("GameDataAssetFactory", "CreateGameDataAssetOptions",
                                    "Pick Game Data Asset Class");
  if (UClass* ChosenClass = nullptr; SClassPickerDialog::PickClass(TitleText, Options, ChosenClass,
                                                                   UGameDataAsset::StaticClass())) {
    AssetClass = ChosenClass;
    return true;
  }

  return false;
}

UObject* UGameDataAssetFactory::FactoryCreateNew(UClass* InClass, UObject* InParent, const FName InName,
                                                 const EObjectFlags Flags,
                                                 UObject* Context, FFeedbackContext* Warn) {
  return NewObject<UGameDataAsset>(InParent, AssetClass, InName, Flags);
}
