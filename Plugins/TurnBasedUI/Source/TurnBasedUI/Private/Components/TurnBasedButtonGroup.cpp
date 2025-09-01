// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/TurnBasedButtonGroup.h"

void UTurnBasedButtonGroup::OnWidgetAdded(UWidget* Widget)
{
    Super::OnWidgetAdded(Widget);
    OnPlaceButton.Broadcast(Buttons.Num(), CastChecked<UCommonButtonBase>(Widget));
}

void UTurnBasedButtonGroup::OnWidgetRemoved(UWidget* Widget)
{
    Super::OnWidgetRemoved(Widget);
    Widget->RemoveFromParent();
}
