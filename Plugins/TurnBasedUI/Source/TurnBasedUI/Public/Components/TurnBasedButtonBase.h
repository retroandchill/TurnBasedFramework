// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonButtonBase.h"
#include "TurnBasedButtonBase.generated.h"

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDUI_API UTurnBasedButtonBase : public UCommonButtonBase
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, Category = "Display")
    FText GetButtonText() const;
    
    UFUNCTION(BlueprintCallable, Category = "Display")
    void SetButtonText(FText InText);

protected:
    void NativePreConstruct() override;
    void UpdateInputActionWidget() override;
    void OnInputMethodChanged(ECommonInputType CurrentInputType) override;
    
    UFUNCTION(meta = (ScriptMethod))
    void RefreshButtonText();

    UFUNCTION(BlueprintImplementableEvent, Category = "Display")
    void UpdateButtonText(const FText& InText);

    UFUNCTION(BlueprintImplementableEvent, Category = "Display")
    void UpdateButtonStyle();

private:
    UPROPERTY(EditAnywhere, Category = "Display", meta = (ScriptName = "ButtonTextOverride"))
    TOptional<FText> ButtonText;
};
