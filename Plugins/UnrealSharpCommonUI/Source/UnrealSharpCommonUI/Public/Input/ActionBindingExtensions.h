// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
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

USTRUCT(BlueprintType)
struct FBindUIActionArgsRef
{
    GENERATED_BODY()

    FBindUIActionArgsRef() = default;
    explicit FBindUIActionArgsRef(FBindUIActionArgs& InRef) : Ref(&InRef) {}

    const FBindUIActionArgs& Get() const
    {
        check(Ref != nullptr);
        return *Ref;
    }

private:
    FBindUIActionArgs* Ref = nullptr;
};

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
    static FUIActionBindingHandle RegisterActionBinding(UCommonUserWidget* Widget, const FCSBindUIActionArgs& Args,
        const FManagedBindUIActionDelegates& Callbacks);

    UFUNCTION(meta = (ScriptMethod))
    static void AddActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod))
    static void RemoveActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle);
};