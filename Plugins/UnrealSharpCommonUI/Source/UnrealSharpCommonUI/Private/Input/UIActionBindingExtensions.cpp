// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/UIActionBindingExtensions.h"

#include <bit>

#include "Input/UIActionBindingHandle.h"

/**
 * This is a mirror type of FUIActionBindingHandle where the two private members are public, used to get data
 * that is otherwise inaccessible to us.
 */
struct FManagedUIActionBindingHandle
{
    FManagedUIActionBindingHandle() = default;
    explicit FManagedUIActionBindingHandle(const int32 RegistrationId) : RegistrationId(RegistrationId) {}
    
#if !UE_BUILD_SHIPPING
    // Using FString since the FName visualizer gets confused after live coding atm
    FString CachedDebugActionName;
#endif

    int32 RegistrationId = INDEX_NONE;
};

// Use this to ensure that we're operating on the same sized object.
// TODO: When P2996R4 is officially available either switch to use reflection outright,
// or use this to verify the two types share the same layout
static_assert(sizeof(FUIActionBindingHandle) == sizeof(FManagedUIActionBindingHandle));

int32 UUIActionBindingExtensions::GetRegistrationId(FUIActionBindingHandleRef HandleRef)
{
    // ReSharper disable once CppUseStructuredBinding
    const auto &Action = *std::bit_cast<FManagedUIActionBindingHandle*>(&HandleRef.Get());
    return Action.RegistrationId;
}

void UUIActionBindingExtensions::SetRegistrationId(FUIActionBindingHandleRef HandleRef, const int32 RegistrationId)
{
    // ReSharper disable once CppUseStructuredBinding
    auto &Action = *std::bit_cast<FManagedUIActionBindingHandle*>(&HandleRef.Get());
    Action = FManagedUIActionBindingHandle(RegistrationId);
}

bool UUIActionBindingExtensions::IsValid(const FUIActionBindingHandle& Handle)
{
    return Handle.IsValid();
}

void UUIActionBindingExtensions::Unregister(FUIActionBindingHandle& Handle)
{
    Handle.Unregister();  
}

void UUIActionBindingExtensions::ResetHold(FUIActionBindingHandle& Handle)
{
    Handle.ResetHold(); 
}

FName UUIActionBindingExtensions::GetActionName(const FUIActionBindingHandle& Handle)
{
    return Handle.GetActionName();
}

FText UUIActionBindingExtensions::GetDisplayName(const FUIActionBindingHandle& Handle)
{
    return Handle.GetDisplayName();
}

void UUIActionBindingExtensions::SetDisplayName(FUIActionBindingHandle& Handle, const FText& DisplayName)
{
    return Handle.SetDisplayName(DisplayName);
}

bool UUIActionBindingExtensions::GetDisplayInActionBar(const FUIActionBindingHandle& Handle)
{
    return Handle.GetDisplayInActionBar();
}

void UUIActionBindingExtensions::SetDisplayInActionBar(FUIActionBindingHandle& Handle,
                                                       const bool bDisplayInActionBar)
{
    Handle.SetDisplayInActionBar(bDisplayInActionBar);
}

const UWidget* UUIActionBindingExtensions::GetBoundWidget(const FUIActionBindingHandle& Handle)
{
    return Handle.GetBoundWidget();
}

ULocalPlayer* UUIActionBindingExtensions::GetBoundLocalPlayer(const FUIActionBindingHandle& Handle)
{
    return Handle.GetBoundLocalPlayer();
}
