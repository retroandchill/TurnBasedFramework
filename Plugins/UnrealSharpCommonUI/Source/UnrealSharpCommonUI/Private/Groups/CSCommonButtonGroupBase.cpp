// Fill out your copyright notice in the Description page of Project Settings.


#include "Groups/CSCommonButtonGroupBase.h"

void UCSCommonButtonGroupBase::OnWidgetAdded(UWidget* NewWidget)
{
    Super::OnWidgetAdded(NewWidget);

    if (auto* Button = Cast<UCommonButtonBase>(NewWidget); Button != nullptr)
    {
        OnButtonAdded(Button);
    }
}

void UCSCommonButtonGroupBase::OnWidgetRemoved(UWidget* OldWidget)
{
    Super::OnWidgetRemoved(OldWidget);
    
    if (auto* Button = Cast<UCommonButtonBase>(OldWidget); Button != nullptr)
    {
        OnButtonRemoved(Button);
    }
}

void UCSCommonButtonGroupBase::OnRemoveAll()
{
    Super::OnRemoveAll();
    K2_OnRemoveAll();  
}
