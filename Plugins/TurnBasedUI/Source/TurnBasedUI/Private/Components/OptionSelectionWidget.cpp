// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/OptionSelectionWidget.h"

#include "Components/TurnBasedButtonBase.h"
#include "Components/TurnBasedButtonGroup.h"
#include "Groups/CommonButtonGroupBase.h"
#include "Misc/DataValidation.h"

void UOptionSelectionWidget::NativePreConstruct()
{
    Super::NativePreConstruct();
    Buttons = NewObject<UTurnBasedButtonGroup>(this);
    PlaceButtonsDelegateHandle = Buttons->BindToPlaceButton(FPlaceButton::FDelegate::CreateWeakLambda(this, [this](const int32 Index, UCommonButtonBase* Button)
    {
        PlaceOptionIntoWidget(Index, Options[Index].Id, CastChecked<UTurnBasedButtonBase>(Button));
    }));
    NativeOptionSelectedDelegateHandle = Buttons->NativeOnButtonBaseClicked.AddWeakLambda(this, [this](UCommonButtonBase* Button, const int32 Index)
    {
        const auto& [Id, Text, InputAction] = Options[Index];
        auto *NewButton = CastChecked<UTurnBasedButtonBase>(Button);
        NativeOptionSelectedDelegate.Broadcast(Index, Id, NewButton);
        OnOptionSelected.Broadcast(Index, Id, NewButton);
    });
}

void UOptionSelectionWidget::NativeDestruct()
{
    Super::NativeDestruct();
    Buttons->UnbindFromPlaceButton(PlaceButtonsDelegateHandle);
    Buttons->NativeOnButtonBaseClicked.Remove(NativeOptionSelectedDelegateHandle);
}

void UOptionSelectionWidget::SetOptions(TArray<FSelectableOption> NewOptions)
{
    Options = MoveTemp(NewOptions);
}

#if WITH_EDITOR
EDataValidationResult UOptionSelectionWidget::IsDataValid(FDataValidationContext& Context) const
{
    auto OriginalResult = Super::IsDataValid(Context);

    if (ButtonClass == nullptr)
    {
        Context.AddError(NSLOCTEXT("TurnBasedUI", "OptionSelectionWidget_ButtonClass_Error", "Button class is not set"));
        OriginalResult = EDataValidationResult::Invalid;
    }

    if (OverrideButtonStyle.IsSet() && !IsValid(*OverrideButtonStyle))
    {
        Context.AddError(NSLOCTEXT("TurnBasedUI", "OptionSelectionWidget_ButtonStyle_Error", "Button style is not set"));
        OriginalResult = EDataValidationResult::Invalid;   
    }

    return OriginalResult;
}
#endif

FDelegateHandle UOptionSelectionWidget::BindToOptionSelected(FNativeOptionSelected::FDelegate Delegate)
{
    return NativeOptionSelectedDelegate.Add(MoveTemp(Delegate));
}

void UOptionSelectionWidget::UnbindFromOptionSelected(const FDelegateHandle Handle)
{
    NativeOptionSelectedDelegate.Remove(Handle);   
}

void UOptionSelectionWidget::CreateOptions()
{
    Buttons->RemoveAll();
    for (int32 i = 0; i < Options.Num(); ++i)
    {
        auto &[Id, Text, InputAction] = Options[i];
        auto *NewButton = CreateWidget<UTurnBasedButtonBase>(this, ButtonClass);
        if (Text.IsSet())
        {
            NewButton->SetButtonText(*Text);
        }

        if (InputAction != nullptr)
        {
            NewButton->SetTriggeringEnhancedInputAction(InputAction);
        }

        Buttons->AddWidget(NewButton);
    }
}
