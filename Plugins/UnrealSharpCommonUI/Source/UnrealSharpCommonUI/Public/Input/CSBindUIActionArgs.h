// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonInputModeTypes.h"
#include "CommonInputTypeEnum.h"
#include "CSManagedGCHandle.h"
#include "UITag.h"
#include "UObject/Object.h"
#include "CSBindUIActionArgs.generated.h"

class UInputAction;

UENUM()
enum class ECSBindUIMode
{
    ActionTag,
    ActionTableRow,
    InputAction
};

/**
 * 
 */
USTRUCT()
struct FCSBindUIActionArgs
{
    GENERATED_BODY()

    UPROPERTY()
    ECSBindUIMode BindUIMode;

    UPROPERTY()
    FUIActionTag ActionTag;

    UPROPERTY()
    FDataTableRowHandle ActionTableRow;

    UPROPERTY()
    TObjectPtr<const UInputAction> InputAction;

    UPROPERTY()
    ECommonInputMode InputMode = ECommonInputMode::Menu;

    UPROPERTY()
    TEnumAsByte<EInputEvent> KeyEvent = IE_Pressed;

    UPROPERTY()
    TSet<ECommonInputType> InputTypesExemptFromValidKeyCheck = { ECommonInputType::MouseAndKeyboard, ECommonInputType::Touch };

    UPROPERTY()
    bool bIsPersistent = false;

    UPROPERTY()
    bool bConsumeInput = true;

    UPROPERTY()
    bool bDisplayInActionBar = true;

    UPROPERTY()
    bool bForceHold = false;

    UPROPERTY()
    FText OverrideDisplayName;

    UPROPERTY()
    int32 PriorityWithinCollection = 0;
};

USTRUCT()
struct FManagedBindUIActionDelegates
{
    GENERATED_BODY()

    FGCHandleIntPtr OnExecuteAction;
    FGCHandleIntPtr OnHoldActionPressed;
    FGCHandleIntPtr OnHoldActionProgressed;
    FGCHandleIntPtr OnHoldActionReleased;
};
