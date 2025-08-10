// Fill out your copyright notice in the Description page of Project Settings.


#include "Customization/PokemonEditorSettingsCustomization.h"

#include "DetailLayoutBuilder.h"
#include "DetailWidgetRow.h"
#include "Interop/PokemonManagedCallbacks.h"

struct FGameplayTag;
static const FName PropertyName = "EvolutionConditionToGameplayTag";

TSharedRef<IDetailCustomization> FPokemonEditorSettingsCustomization::MakeInstance()
{
    return MakeShared<FPokemonEditorSettingsCustomization>();
}

void FPokemonEditorSettingsCustomization::CustomizeDetails(IDetailLayoutBuilder& DetailBuilder)
{
    const auto EvolutionConditionProperty = DetailBuilder.GetProperty(PropertyName);

    TArray<TWeakObjectPtr<>> ObjectsBeingCustomized;
    DetailBuilder.GetObjectsBeingCustomized(ObjectsBeingCustomized);
    const auto PropertyEditor = DetailBuilder.EditDefaultProperty(EvolutionConditionProperty);
    PropertyEditor->ShowPropertyButtons(false);
    PropertyEditor->CustomWidget(true).NameContent()
    [
        EvolutionConditionProperty->CreatePropertyNameWidget()
    ]
    .ValueContent()
    [
        SNew(SHorizontalBox)
            +SHorizontalBox::Slot()
                .AutoWidth()
                .VAlign(VAlign_Fill)
                [
                    EvolutionConditionProperty->CreatePropertyValueWidget()
                ]
            +SHorizontalBox::Slot()
                .AutoWidth()
                .VAlign(VAlign_Fill)
                [
                    SNew(SButton)
                        .Text(NSLOCTEXT("UnrealSharpEditor", "AutoPopulate", "Auto Populate from Evolution Table"))
                        .OnClicked_Static(&FPokemonEditorSettingsCustomization::OnAutoPopulateClicked, EvolutionConditionProperty,
                            const_cast<const UClass*>(DetailBuilder.GetBaseClass()), MoveTemp(ObjectsBeingCustomized))
                ]
    ];
}

FReply FPokemonEditorSettingsCustomization::OnAutoPopulateClicked(const TSharedRef<IPropertyHandle> EditedProperty, const UClass* BaseClass, TArray<TWeakObjectPtr<>> Objects)
{
    EditedProperty->NotifyPreChange();
    const auto TargetProperty = CastFieldChecked<FMapProperty>(BaseClass->FindPropertyByName(PropertyName));
    for (auto Object : Objects)
    {
        auto &TargetMap = TargetProperty->GetPropertyValue_InContainer(Object.Get());
        if (auto Result = FPokemonManagedCallbacks::Get().PopulateEvolutions(TargetProperty, TargetMap); !Result.has_value())
        {
            FMessageDialog::Open(EAppMsgType::Ok, FText::FromString(Result.error()));
        }
    }
    EditedProperty->NotifyPostChange(EPropertyChangeType::ValueSet);
    
    return FReply::Handled();
}
