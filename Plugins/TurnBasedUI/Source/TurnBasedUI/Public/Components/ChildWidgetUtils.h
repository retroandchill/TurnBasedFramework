// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "ChildWidgetUtils.generated.h"

/**
 * 
 */
UCLASS()
class TURNBASEDUI_API UChildWidgetUtils : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(meta = (ScriptMethod, DynamicOutputParam = ReturnValue, DeterminesOutputType = WidgetClass))
    static UUserWidget* CreateChildWidget(UUserWidget* Component, TSubclassOf<UUserWidget> WidgetClass);
};
