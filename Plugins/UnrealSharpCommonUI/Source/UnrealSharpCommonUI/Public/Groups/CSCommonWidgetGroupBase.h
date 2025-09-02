// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Groups/CommonWidgetGroupBase.h"
#include "CSCommonWidgetGroupBase.generated.h"

/**
 * 
 */
UCLASS()
class UNREALSHARPCOMMONUI_API UCSCommonWidgetGroupBase : public UCommonWidgetGroupBase
{
    GENERATED_BODY()

protected:
    void OnWidgetAdded(UWidget* NewWidget) override;
    void OnWidgetRemoved(UWidget* OldWidget) override;
    void OnRemoveAll() override;

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group", meta = (ScriptName = "OnWidgetAdded"))
    void K2_OnWidgetAdded(UWidget* NewButton);

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group", meta = (ScriptName = "OnWidgetRemoved"))
    void K2_OnWidgetRemoved(UWidget* OldButton);

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group", meta = (ScriptName = "OnRemoveAll"))
    void K2_OnRemoveAll();
};
