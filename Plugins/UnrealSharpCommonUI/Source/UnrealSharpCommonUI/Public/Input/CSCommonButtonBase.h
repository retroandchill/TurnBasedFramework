// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonButtonBase.h"
#include "CSCommonButtonBase.generated.h"

/**
 * 
 */
UCLASS(Abstract, ClassGroup = UI, meta = (Category = "Common UI", DisableNativeTick))
class UNREALSHARPCOMMONUI_API UCSCommonButtonBase : public UCommonButtonBase
{
    GENERATED_BODY()

public:
    void UpdateInputActionWidget() override;

protected:
    UFUNCTION(BlueprintImplementableEvent, Category = "Action", meta = (ScriptName = "UpdateInputActionWidget"))
    void K2_UpdateInputActionWidget();
};
