﻿// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUIExtensions.h"

#include "CommonActivatableWidget.h"
#include "CommonInputSubsystem.h"
#include "OptionalPtr.h"
#include "PrimaryGameLayout.h"
#include "TurnBasedUIManagerSubsystem.h"
#include "TurnBasedUIPolicy.h"
#include "Blueprint/UserWidget.h"

int32 UTurnBasedUIExtensions::InputSuspensions = 0;

ECommonInputType UTurnBasedUIExtensions::GetOwningPlayerInputType(const UUserWidget* WidgetContextObject)
{
    return TOptionalPtr(WidgetContextObject)
        .Map([](const UUserWidget* Widget) { return Widget->GetOwningLocalPlayer(); })
        .Map(&UCommonInputSubsystem::Get)
        .MapToValue(ECommonInputType::Count, &UCommonInputSubsystem::GetCurrentInputType);
}

static bool IsOwningPlayerInputType(const UUserWidget* WidgetContextObject, ECommonInputType InputType)
{
    return TOptionalPtr(WidgetContextObject)
        .Map([](const UUserWidget* Widget) { return Widget->GetOwningLocalPlayer(); })
        .Map(&UCommonInputSubsystem::Get)
        .MapToValue(false, [InputType](const UCommonInputSubsystem* Subsystem) { return Subsystem->GetCurrentInputType() == InputType; });
}

bool UTurnBasedUIExtensions::IsOwningPlayerUsingTouch(const UUserWidget* WidgetContextObject)
{
    return IsOwningPlayerInputType(WidgetContextObject, ECommonInputType::Touch);
        
}

bool UTurnBasedUIExtensions::IsOwningPlayerUsingGamepad(const UUserWidget* WidgetContextObject)
{
    return IsOwningPlayerInputType(WidgetContextObject, ECommonInputType::Gamepad);
}

UCommonActivatableWidget* UTurnBasedUIExtensions::PushContentToLayer_ForPlayer(const ULocalPlayer* LocalPlayer,
    FGameplayTag LayerName, TSubclassOf<UCommonActivatableWidget> WidgetClass)
{
    if (ensure(LocalPlayer != nullptr) || !ensure(WidgetClass != nullptr))
    {
        return nullptr;
    }

    return TOptionalPtr(LocalPlayer->GetGameInstance()->GetSubsystem<UTurnBasedUIManagerSubsystem>())
        .Map([](UTurnBasedUIManagerSubsystem* Subsystem) { return Subsystem->GetCurrentUIPolicy(); })
        .Map([LocalPlayer](const UTurnBasedUIPolicy* Policy) { return Policy->GetRootLayout(LocalPlayer); })
        .Map([LayerName, WidgetClass](UPrimaryGameLayout* Layout) { return Layout->PushWidgetToLayerStack(LayerName, WidgetClass); })
        .Get();
}

void UTurnBasedUIExtensions::PushStreamedContentToLayer_ForPlayer(const ULocalPlayer* LocalPlayer,
    FGameplayTag LayerName, TSoftClassPtr<UCommonActivatableWidget> WidgetClass)
{
    if (ensure(LocalPlayer != nullptr) || !ensure(WidgetClass != nullptr))
    {
        return;
    }

    TOptionalPtr(LocalPlayer->GetGameInstance()->GetSubsystem<UTurnBasedUIManagerSubsystem>())
        .Map([](UTurnBasedUIManagerSubsystem* Subsystem) { return Subsystem->GetCurrentUIPolicy(); })
        .Map([LocalPlayer](const UTurnBasedUIPolicy* Policy) { return Policy->GetRootLayout(LocalPlayer); })
        .IfPresent ([LayerName, &WidgetClass](UPrimaryGameLayout* Layout)
        {
            return Layout->PushWidgetToLayerStackAsync(LayerName, true, MoveTemp(WidgetClass));
        });
}

void UTurnBasedUIExtensions::PopContentFromLayer(UCommonActivatableWidget* ActivatableWidget)
{
    if (ActivatableWidget == nullptr)
    {
        // Ignore request to pop an already deleted widget
        return;
    }

    const auto LocalPlayer = TOptionalPtr(ActivatableWidget)
        .Map([](const UCommonActivatableWidget* Widget) { return Widget->GetOwningLocalPlayer(); })
        .Get();
    
    TOptionalPtr(LocalPlayer)
        .Map([](const ULocalPlayer* Player) { return Player->GetGameInstance()->GetSubsystem<UTurnBasedUIManagerSubsystem>(); })
        .Map([](const UTurnBasedUIManagerSubsystem* Subsystem) { return Subsystem->GetCurrentUIPolicy(); })
        .Map([LocalPlayer](const UTurnBasedUIPolicy* Policy) { return Policy->GetRootLayout(LocalPlayer); })
        .IfPresent([ActivatableWidget](UPrimaryGameLayout* Layout)
        {
            Layout->FindAndRemoveWidgetFromLayer(ActivatableWidget);
        });
}

ULocalPlayer* UTurnBasedUIExtensions::GetLocalPlayerFromController(APlayerController* PlayerController)
{
    return PlayerController != nullptr ? PlayerController->GetLocalPlayer() : nullptr;
}

FName UTurnBasedUIExtensions::SuspendInputForPlayer(APlayerController* PlayerController, const FName SuspendReason)
{
    return SuspendInputForPlayer(GetLocalPlayerFromController(PlayerController), SuspendReason);
}

FName UTurnBasedUIExtensions::SuspendInputForPlayer(const ULocalPlayer* LocalPlayer, const FName SuspendReason)
{
    const auto CommonInputSubsystem = UCommonInputSubsystem::Get(LocalPlayer);
    if (CommonInputSubsystem == nullptr)
    {
        return NAME_None;
    }
    
    InputSuspensions++;
    FName SuspendToken = SuspendReason;
    SuspendToken.SetNumber(InputSuspensions);

    CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::MouseAndKeyboard, SuspendToken, true);
    CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::Gamepad, SuspendToken, true);
    CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::Touch, SuspendToken, true);

    return SuspendToken;
}

void UTurnBasedUIExtensions::ResumeInputForPlayer(APlayerController* PlayerController, const FName SuspendToken)
{
    ResumeInputForPlayer(GetLocalPlayerFromController(PlayerController), SuspendToken);
}

void UTurnBasedUIExtensions::ResumeInputForPlayer(const ULocalPlayer* LocalPlayer, const FName SuspendToken)
{
    if (SuspendToken == NAME_None)
    {
        return;
    }

    if (auto* CommonInputSubsystem = UCommonInputSubsystem::Get(LocalPlayer); CommonInputSubsystem != nullptr)
    {
        CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::MouseAndKeyboard, SuspendToken, false);
        CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::Gamepad, SuspendToken, false);
        CommonInputSubsystem->SetInputTypeFilter(ECommonInputType::Touch, SuspendToken, false);
    }
}
