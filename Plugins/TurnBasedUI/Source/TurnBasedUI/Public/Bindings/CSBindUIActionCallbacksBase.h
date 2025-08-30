// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Input/UIActionBindingHandle.h"
#include "UObject/Object.h"
#include "CSBindUIActionCallbacksBase.generated.h"

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDUI_API UCSBindUIActionCallbacksBase : public UObject
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintImplementableEvent, Category = "UI Actions")
    void InvokeOnExecuteAction();
    
    UFUNCTION(BlueprintImplementableEvent, Category = "UI Actions")
    void InvokeOnHoldActionProgressed(float Progress);
    
    UFUNCTION(BlueprintImplementableEvent, Category = "UI Actions")
    void InvokeOnHoldActionPressed();
    
    UFUNCTION(BlueprintImplementableEvent, Category = "UI Actions")
    void InvokeOnHoldActionReleased();
};