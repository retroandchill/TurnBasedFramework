// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "IDetailCustomization.h"

/**
 * 
 */
class POKEMONEDITORTOOLS_API FPokemonEditorSettingsCustomization final : public IDetailCustomization
{
public:
    static TSharedRef<IDetailCustomization> MakeInstance();

    void CustomizeDetails(IDetailLayoutBuilder& DetailBuilder) override;

private:
    static FReply OnAutoPopulateClicked(const TSharedRef<IPropertyHandle> EditedProperty, const UClass* BaseClass, TArray<TWeakObjectPtr<>> Objects);
};
