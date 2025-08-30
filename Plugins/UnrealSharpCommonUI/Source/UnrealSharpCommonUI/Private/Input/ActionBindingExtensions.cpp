// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/ActionBindingExtensions.h"

#include "CommonUserWidget.h"
#include "CSCommonUISubsystem.h"
#include "Unreachable.h"
#include "Input/CommonUIInputTypes.h"
#include "Input/CSBindUIActionArgs.h"
#include "Input/CSInputBindingCallbacks.h"
#include "Kismet/GameplayStatics.h"

static FBindUIActionArgs ConstructNativeArgs(const FCSBindUIActionArgs& Args,
                                             const TSharedRef<FCSInputBindingCallbacks>& Callbacks)
{
    TWeakPtr<FCSInputBindingCallbacks> WeakCallbacks = Callbacks;
    auto OnExecuteAction = FSimpleDelegate::CreateLambda([WeakCallbacks]
    {
        if (const auto StrongCallbacks = WeakCallbacks.Pin(); StrongCallbacks.IsValid())
       {
           StrongCallbacks->InvokeOnExecuteAction();
       } 
    });
    switch (Args.BindUIMode)
    {
    case ECSBindUIMode::ActionTag:
        return FBindUIActionArgs(Args.ActionTag, Args.bDisplayInActionBar, MoveTemp(OnExecuteAction));
    case ECSBindUIMode::ActionTableRow:
        return FBindUIActionArgs(Args.ActionTableRow, Args.bDisplayInActionBar, MoveTemp(OnExecuteAction));
    case ECSBindUIMode::InputAction:
        return FBindUIActionArgs(Args.InputAction, Args.bDisplayInActionBar, MoveTemp(OnExecuteAction));
    default:
        UE_LOG(LogTemp, Error, TEXT("Unknown bind mode"));
        UE::Unreachable();
    }
}

static FGCHandle ConvertHandle(const FGCHandleIntPtr Ptr)
{
    return Ptr.IntPtr != nullptr ? FGCHandle(Ptr.IntPtr, GCHandleType::StrongHandle) : FGCHandle::Null();
}

const TArray<FUIActionBindingHandle>& UActionBindingExtensions::GetActionBindings(const UCommonUserWidget* Widget)
{
    return Widget->GetActionBindings();
}

FUIActionBindingHandle UActionBindingExtensions::RegisterActionBinding(UCommonUserWidget* Widget,
    const FCSBindUIActionArgs& Args, const FManagedBindUIActionDelegates& Callbacks)
{
    auto Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    check(Subsystem != nullptr);

    const auto BindCallbacks = Subsystem->BindInputActionCallbacks(Widget,
        ConvertHandle(Callbacks.OnExecuteAction),
        ConvertHandle(Callbacks.OnHoldActionPressed),
        ConvertHandle(Callbacks.OnHoldActionReleased),
        ConvertHandle(Callbacks.OnHoldActionProgressed));
    
    auto NativeArgs = ConstructNativeArgs(Args, BindCallbacks);

    NativeArgs.InputMode = Args.InputMode;
    NativeArgs.KeyEvent = Args.KeyEvent;
    NativeArgs.InputTypesExemptFromValidKeyCheck = Args.InputTypesExemptFromValidKeyCheck;
    NativeArgs.bIsPersistent = Args.bIsPersistent;
    NativeArgs.bConsumeInput = Args.bConsumeInput;
    NativeArgs.bForceHold = Args.bForceHold;
    NativeArgs.OverrideDisplayName = Args.OverrideDisplayName;
    NativeArgs.PriorityWithinCollection = Args.PriorityWithinCollection;
    
    return Widget->RegisterUIActionBinding(NativeArgs);
}

void UActionBindingExtensions::AddActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle)
{
    Widget->AddActionBinding(Handle);
}

void UActionBindingExtensions::RemoveActionBinding(UCommonUserWidget* Widget, const FUIActionBindingHandle& Handle)
{
    Widget->RemoveActionBinding(Handle);
}
