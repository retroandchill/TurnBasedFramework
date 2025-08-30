// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindUIActionArgs.h"
#include "CSInputBindingCallbacks.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "BindUIActionArgsExtensions.generated.h"

class UWidget;
/**
 * 
 */
UCLASS(meta = (InternalType))
class UNREALSHARPCOMMONUI_API UBindUIActionArgsExtensions : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(meta = (ScriptMethod))
    static int32 GetBindUIActionArgsSize();
    
    UFUNCTION(meta = (ScriptMethod))
    static void ConstructFromActionTag(FBindUIActionArgsRef Args, UWidget* Widget, FUIActionTag InActionTag, bool bShouldDisplayActionInBar, FManagedDelegateHandle InOnExecuteAction);
    
    UFUNCTION(meta = (ScriptMethod))
    static void ConstructFromRowHandle(FBindUIActionArgsRef Args, UWidget* Widget,  FDataTableRowHandle Handle, bool bShouldDisplayActionInBar, FManagedDelegateHandle InOnExecuteAction);
    
    UFUNCTION(meta = (ScriptMethod))
    static void ConstructFromInputAction(FBindUIActionArgsRef Args, UWidget* Widget, const UInputAction* InInputAction, bool bShouldDisplayActionInBar, FManagedDelegateHandle InOnExecuteAction);

    UFUNCTION(meta = (ScriptMethod))
    static void Copy(FBindUIActionArgsRef Src, FBindUIActionArgsRef Dst);
    
    UFUNCTION(meta = (ScriptMethod))
    static void Release(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static FName GetActionName(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static bool ActionHasHoldMappings(FBindUIActionArgsRef Args);
    
    UFUNCTION(meta = (ScriptMethod))
    static FUIActionTag GetActionTag(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static const FDataTableRowHandle& GetActionTableRow(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static const UInputAction* GetInputAction(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static ECommonInputMode GetInputMode(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetInputMode(FBindUIActionArgsRef Args, ECommonInputMode InputMode);

    UFUNCTION(meta = (ScriptMethod))
    static EInputEvent GetKeyEvent(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetKeyEvent(FBindUIActionArgsRef Args, EInputEvent KeyEvent);

    UFUNCTION(meta = (ScriptMethod))
    static const TSet<ECommonInputType> & GetInputTypesExemptFromValidKeyCheck(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetInputTypesExemptFromValidKeyCheck(FBindUIActionArgsRef Args, TSet<ECommonInputType> InputTypes);

    UFUNCTION(meta = (ScriptMethod))
    static bool GetIsPersistent(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetIsPersistent(FBindUIActionArgsRef Args, bool bIsPersistent);

    UFUNCTION(meta = (ScriptMethod))
    static bool GetConsumeInput(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetConsumeInput(FBindUIActionArgsRef Args, bool bConsumeInput);

    UFUNCTION(meta = (ScriptMethod))
    static bool GetDisplayInActionBar(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetDisplayInActionBar(FBindUIActionArgsRef Args, bool bDisplayInActionBar);

    UFUNCTION(meta = (ScriptMethod))
    static bool GetForceHold(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetForceHold(FBindUIActionArgsRef Args, bool bForceHold);

    UFUNCTION(meta = (ScriptMethod))
    static const FText& GetOverrideDisplayName(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetOverrideDisplayName(FBindUIActionArgsRef Args, FText OverrideDisplayName);

    UFUNCTION(meta = (ScriptMethod))
    static int32 GetPriorityWithinCollection(FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void SetPriorityWithinCollection(FBindUIActionArgsRef Args, int32 PriorityWithinCollection);

    UFUNCTION(meta = (ScriptMethod))
    static void BindOnHoldActionProgressed(FBindUIActionArgsRef Args, UWidget* Widget, FManagedDelegateHandle InOnHoldActionProgressed, FGuid InId);
    
    UFUNCTION(meta = (ScriptMethod))
    static void BindOnHoldActionPressed(FBindUIActionArgsRef Args, UWidget* Widget, FManagedDelegateHandle InOnHoldActionPressed);

    UFUNCTION(meta = (ScriptMethod))
    static void BindOnHoldActionReleased(FBindUIActionArgsRef Args, UWidget* Widget, FManagedDelegateHandle InOnHoldActionReleased);

    UFUNCTION(meta = (ScriptMethod))
    static FInputBindingCallbackRef GetBoundDelegateData(const UWidget* Widget, FGuid Id);
};
