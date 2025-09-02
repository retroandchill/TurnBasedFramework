// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Groups/CommonButtonGroupBase.h"
#include "CSCommonButtonGroupBase.generated.h"

/**
 * 
 */
UCLASS()
class UNREALSHARPCOMMONUI_API UCSCommonButtonGroupBase : public UCommonButtonGroupBase
{
    GENERATED_BODY()

protected:
    void OnWidgetAdded(UWidget* NewWidget) override;
    void OnWidgetRemoved(UWidget* OldWidget) override;
    void OnRemoveAll() override;

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group")
    void OnButtonAdded(UCommonButtonBase* NewButton);

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group")
    void OnButtonRemoved(UCommonButtonBase* OldButton);

    UFUNCTION(BlueprintImplementableEvent, Category = "Button Group", meta = (ScriptName = "OnRemoveAll"))
    void K2_OnRemoveAll();
};
