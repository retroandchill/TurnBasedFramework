// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "ActionRegistrationExporter.generated.h"

struct FUIActionBindingHandle;
struct FBindUIActionArgs;
class UCommonUserWidget;
/**
 * 
 */
UCLASS()
class TURNBASEDUI_API UActionRegistrationExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void GetActionBindings(const UCommonUserWidget* Widget, const FUIActionBindingHandle*& Bindings, int32& NumBindings);

    UNREALSHARP_FUNCTION()
    static void RegisterActionBinding(UCommonUserWidget* Widget, const FBindUIActionArgs& Args, FUIActionBindingHandle* Handle);

    UNREALSHARP_FUNCTION()
    static void AddActionBinding(UCommonUserWidget* Widget, FUIActionBindingHandle* Handle);

    UNREALSHARP_FUNCTION()
    static void RemoveActionBinding(UCommonUserWidget* Widget, FUIActionBindingHandle* Handle);

    UNREALSHARP_FUNCTION()
    static void DestructHandle(FUIActionBindingHandle* Handle);
};
