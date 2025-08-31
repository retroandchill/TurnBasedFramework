// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "UIActionBindingExtensions.generated.h"

class UWidget;
struct FUIActionBindingHandle;

USTRUCT(meta = (InternalType))
struct FUIActionBindingHandleRef
{
    GENERATED_BODY()
    
    FUIActionBindingHandleRef() = default;
    explicit FUIActionBindingHandleRef(FUIActionBindingHandle& InHandle) : Handle(&InHandle) {}

    FUIActionBindingHandle& Get()
    {
        return *Handle;
    }

    const FUIActionBindingHandle& Get() const
    {
        return *Handle;
    }
    
private:
    FUIActionBindingHandle* Handle = nullptr;
};

/**
 * 
 */
UCLASS(meta = (InternalType))
class UNREALSHARPCOMMONUI_API UUIActionBindingExtensions : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static int32 GetRegistrationId(FUIActionBindingHandleRef HandleRef);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static void SetRegistrationId(FUIActionBindingHandleRef HandleRef, int32 RegistrationId);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static bool IsValid(const FUIActionBindingHandle& Handle);
    
    UFUNCTION(meta = (ScriptMethod))
    static void Unregister(UPARAM(ref) FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod))
    static void ResetHold(UPARAM(ref) FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static FName GetActionName(const FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static FText GetDisplayName(const FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod))
    static void SetDisplayName(UPARAM(ref) FUIActionBindingHandle& Handle, const FText& DisplayName);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static bool GetDisplayInActionBar(const FUIActionBindingHandle& Handle);

    UFUNCTION(meta = (ScriptMethod))
    static void SetDisplayInActionBar(UPARAM(ref) FUIActionBindingHandle& Handle, const bool bDisplayInActionBar);

    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static const UWidget* GetBoundWidget(const FUIActionBindingHandle& Handle);
	
    UFUNCTION(meta = (ScriptMethod, ExtensionMethod))
    static ULocalPlayer* GetBoundLocalPlayer(const FUIActionBindingHandle& Handle);
};
