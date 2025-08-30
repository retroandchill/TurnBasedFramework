// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindUIActionArgs.h"
#include "Input/UIActionBindingHandle.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "ActionBindingExtensions.generated.h"

struct FManagedBindUIActionDelegates;
struct FCSBindUIActionArgs;
class UInputAction;
class UCSBindUIActionCallbacksBase;
class UCommonUserWidget;
class FCSInputBindingCallbacks;
struct FBindUIActionArgs;

/**
 * 
 */
UCLASS(meta = (InternalType))
class UNREALSHARPCOMMONUI_API UActionBindingExtensions : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(meta = (ScriptMethod))
    static const TArray<FUIActionBindingHandle>& GetActionBindings(const UCommonUserWidget* Widget);

    UFUNCTION(meta = (ScriptMethod))
    static FUIActionBindingHandle RegisterActionBinding(UCommonUserWidget* Widget, FBindUIActionArgsRef Args);

    UFUNCTION(meta = (ScriptMethod))
    static void AddActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod))
    static void RemoveActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle);
};