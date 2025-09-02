// Fill out your copyright notice in the Description page of Project Settings.


#include "Components/ChildWidgetUtils.h"

#include "Blueprint/UserWidget.h"

UUserWidget* UChildWidgetUtils::CreateChildWidget(UUserWidget* Component, const TSubclassOf<UUserWidget> WidgetClass)
{
    return CreateWidget(Component, WidgetClass);
}
