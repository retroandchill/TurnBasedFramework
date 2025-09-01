// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/TurnBasedButtonBase.h"

#include "CommonActionWidget.h"

FText UTurnBasedButtonBase::GetButtonText() const
{
    if (ButtonText.IsSet())
    {
        return *ButtonText;
    }

    if (InputActionWidget == nullptr) return FText::GetEmpty();

    return InputActionWidget->GetDisplayText();
}

void UTurnBasedButtonBase::SetButtonText(FText InText)
{
    if (InText.IsEmpty())
    {
        ButtonText.Reset();       
    }
    else
    {
        ButtonText.Emplace(MoveTemp(InText));
    }
}

void UTurnBasedButtonBase::NativePreConstruct()
{
    Super::NativePreConstruct();

    UpdateButtonStyle();
    RefreshButtonText();
}

void UTurnBasedButtonBase::UpdateInputActionWidget()
{
    Super::UpdateInputActionWidget();
}

void UTurnBasedButtonBase::OnInputMethodChanged(ECommonInputType CurrentInputType)
{
    Super::OnInputMethodChanged(CurrentInputType);
    UpdateButtonStyle();
}

void UTurnBasedButtonBase::RefreshButtonText()
{
    UpdateButtonText(GetButtonText());   
}
