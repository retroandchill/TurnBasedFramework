// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/ActionBindingExtensions.h"

#include "CommonUserWidget.h"

const TArray<FUIActionBindingHandle>& UActionBindingExtensions::GetActionBindings(const UCommonUserWidget* Widget)
{
    return Widget->GetActionBindings();
}

FUIActionBindingHandle UActionBindingExtensions::RegisterActionBinding(UCommonUserWidget* Widget,
    const FBindUIActionArgsRef Args)
{
    return Widget->RegisterUIActionBinding(Args.Get());
}

void UActionBindingExtensions::AddActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle)
{
    Widget->AddActionBinding(Handle);
}

void UActionBindingExtensions::RemoveActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle)
{
    Widget->RemoveActionBinding(Handle);
}
