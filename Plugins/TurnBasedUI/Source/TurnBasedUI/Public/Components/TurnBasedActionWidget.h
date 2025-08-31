// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonActionWidget.h"
#include "TurnBasedActionWidget.generated.h"

class UInputAction;
class UEnhancedInputLocalPlayerSubsystem;

/**
 * 
 */
UCLASS(Blueprintable, BlueprintType)
class TURNBASEDUI_API UTurnBasedActionWidget : public UCommonActionWidget
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    UInputAction* GetAssociatedInputAction() const { return AssociatedInputAction.Get(); }

    FSlateBrush GetIcon() const override;
    
private:
    UEnhancedInputLocalPlayerSubsystem* GetEnhancedInputSubsystem() const;
    
    UPROPERTY(EditAnywhere, BlueprintGetter = GetAssociatedInputAction, Category = "Input")
	const TObjectPtr<UInputAction> AssociatedInputAction;
};
