// Fill out your copyright notice in the Description page of Project Settings.


#include "Groups/CSCommonWidgetGroupBase.h"

void UCSCommonWidgetGroupBase::OnWidgetAdded(UWidget* NewWidget)
{
    K2_OnWidgetAdded(NewWidget);
}

void UCSCommonWidgetGroupBase::OnWidgetRemoved(UWidget* OldWidget)
{
    K2_OnWidgetRemoved(OldWidget);  
}

void UCSCommonWidgetGroupBase::OnRemoveAll()
{
    K2_OnRemoveAll();
}
