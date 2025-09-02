// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/BindUIActionArgsExtensions.h"

#include "CSCommonUISubsystem.h"
#include "Components/Widget.h"
#include "Input/CommonUIInputTypes.h"
#include "Kismet/GameplayStatics.h"

DECLARE_DELEGATE_OneParam(FFloatDelegate, float);

template <typename T>
    requires std::constructible_from<FBindUIActionArgs, T, bool, FSimpleDelegate>
static void Construct(FBindUIActionArgsRef Args, UWidget* Widget, T&& Arg, bool bShouldDisplayActionInBar, FManagedDelegateHandle InOnExecuteAction)
{
    auto *Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    const auto Callback = MakeShared<TCSInputBindingCallback<>>(FGCHandle(InOnExecuteAction.Ptr, GCHandleType::StrongHandle));
    Subsystem->RegisterInputBindingCallback(Widget, FGuid::NewGuid(), Callback);
    TWeakPtr<TCSInputBindingCallback<>> WeakCallback = Callback;

    std::construct_at(&Args.Get(), Forward<T>(Arg), bShouldDisplayActionInBar, FSimpleDelegate::CreateWeakLambda(Widget, [Widget, WeakCallback]
    {
        if (const auto Pinned = WeakCallback.Pin(); Pinned != nullptr)
        {
            Pinned->Invoke(Widget);
        }
    }));
}

int32 UBindUIActionArgsExtensions::GetBindUIActionArgsSize()
{
    return sizeof(FBindUIActionArgs);
}

void UBindUIActionArgsExtensions::ConstructFromActionTag(const FBindUIActionArgsRef Args, UWidget* Widget, const FUIActionTag InActionTag,
    const bool bShouldDisplayActionInBar, const FManagedDelegateHandle InOnExecuteAction)
{
    Construct(Args, Widget, InActionTag, bShouldDisplayActionInBar, InOnExecuteAction);
}

void UBindUIActionArgsExtensions::ConstructFromRowHandle(const FBindUIActionArgsRef Args, UWidget* Widget, FDataTableRowHandle Handle,
    const bool bShouldDisplayActionInBar, const FManagedDelegateHandle InOnExecuteAction)
{
    Construct(Args, Widget, Handle, bShouldDisplayActionInBar, InOnExecuteAction);
}

void UBindUIActionArgsExtensions::ConstructFromInputAction(const FBindUIActionArgsRef Args, UWidget* Widget,
    const UInputAction* InInputAction, const bool bShouldDisplayActionInBar, const FManagedDelegateHandle InOnExecuteAction)
{
    Construct(Args, Widget, InInputAction, bShouldDisplayActionInBar, InOnExecuteAction);
}

void UBindUIActionArgsExtensions::Copy(const FBindUIActionArgsRef Src, FBindUIActionArgsRef Dst)
{
    std::construct_at(&Dst.Get(), Src.Get());  
}

void UBindUIActionArgsExtensions::Release(FBindUIActionArgsRef Args)
{
    std::destroy_at(&Args.Get());
}

FName UBindUIActionArgsExtensions::GetActionName(FBindUIActionArgsRef Args)
{
    return Args.Get().GetActionName();
}

bool UBindUIActionArgsExtensions::ActionHasHoldMappings(FBindUIActionArgsRef Args)
{
    return Args.Get().ActionHasHoldMappings();  
}

FUIActionTag UBindUIActionArgsExtensions::GetActionTag(FBindUIActionArgsRef Args)
{
    return Args.Get().ActionTag;
}

const FDataTableRowHandle& UBindUIActionArgsExtensions::GetActionTableRow(FBindUIActionArgsRef Args)
{
    return Args.Get().LegacyActionTableRow;
}

const UInputAction* UBindUIActionArgsExtensions::GetInputAction(FBindUIActionArgsRef Args)
{
    return Args.Get().InputAction.Get();
}

ECommonInputMode UBindUIActionArgsExtensions::GetInputMode(FBindUIActionArgsRef Args)
{
    return Args.Get().InputMode;
}

void UBindUIActionArgsExtensions::SetInputMode(FBindUIActionArgsRef Args, const ECommonInputMode InputMode)
{
    Args.Get().InputMode = InputMode;  
}

EInputEvent UBindUIActionArgsExtensions::GetKeyEvent(FBindUIActionArgsRef Args)
{
    return Args.Get().KeyEvent; 
}

void UBindUIActionArgsExtensions::SetKeyEvent(FBindUIActionArgsRef Args, const EInputEvent KeyEvent)
{
    Args.Get().KeyEvent = KeyEvent; 
}

const TSet<ECommonInputType>& UBindUIActionArgsExtensions::GetInputTypesExemptFromValidKeyCheck(
    FBindUIActionArgsRef Args)
{
    return Args.Get().InputTypesExemptFromValidKeyCheck; 
}

void UBindUIActionArgsExtensions::SetInputTypesExemptFromValidKeyCheck(FBindUIActionArgsRef Args,
    TSet<ECommonInputType> InputTypes)
{
    Args.Get().InputTypesExemptFromValidKeyCheck = MoveTemp(InputTypes);
}

bool UBindUIActionArgsExtensions::GetIsPersistent(FBindUIActionArgsRef Args)
{
    return Args.Get().bIsPersistent;
}

void UBindUIActionArgsExtensions::SetIsPersistent(FBindUIActionArgsRef Args, const bool bIsPersistent)
{
    Args.Get().bIsPersistent = bIsPersistent; 
}

bool UBindUIActionArgsExtensions::GetConsumeInput(FBindUIActionArgsRef Args)
{
    return Args.Get().bConsumeInput;
}

void UBindUIActionArgsExtensions::SetConsumeInput(FBindUIActionArgsRef Args, const bool bConsumeInput)
{
    Args.Get().bConsumeInput = bConsumeInput;
}

bool UBindUIActionArgsExtensions::GetDisplayInActionBar(FBindUIActionArgsRef Args)
{
    return Args.Get().bDisplayInActionBar; 
}

void UBindUIActionArgsExtensions::SetDisplayInActionBar(FBindUIActionArgsRef Args, const bool bDisplayInActionBar)
{
    Args.Get().bDisplayInActionBar = bDisplayInActionBar;
}

bool UBindUIActionArgsExtensions::GetForceHold(FBindUIActionArgsRef Args)
{
    return Args.Get().bForceHold;
}

void UBindUIActionArgsExtensions::SetForceHold(FBindUIActionArgsRef Args, const bool bForceHold)
{
    Args.Get().bForceHold = bForceHold;
}

const FText& UBindUIActionArgsExtensions::GetOverrideDisplayName(FBindUIActionArgsRef Args)
{
    return Args.Get().OverrideDisplayName;
}

void UBindUIActionArgsExtensions::SetOverrideDisplayName(FBindUIActionArgsRef Args, FText OverrideDisplayName)
{
    Args.Get().OverrideDisplayName = MoveTemp(OverrideDisplayName);
}

int32 UBindUIActionArgsExtensions::GetPriorityWithinCollection(FBindUIActionArgsRef Args)
{
    return Args.Get().PriorityWithinCollection;
}

void UBindUIActionArgsExtensions::SetPriorityWithinCollection(FBindUIActionArgsRef Args, const int32 PriorityWithinCollection)
{
    Args.Get().PriorityWithinCollection = PriorityWithinCollection;
}

void UBindUIActionArgsExtensions::BindOnHoldActionProgressed(FBindUIActionArgsRef Args, UWidget* Widget,
                                                             const FManagedDelegateHandle InOnHoldActionProgressed,
                                                             const FGuid InId)
{
    auto *Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    const auto Callback = MakeShared<TCSInputBindingCallback<float>>(FGCHandle(InOnHoldActionProgressed.Ptr, GCHandleType::StrongHandle));
    Subsystem->RegisterInputBindingCallback(Widget, InId, Callback);
    TWeakPtr<TCSInputBindingCallback<float>> WeakCallback = Callback;

    Args.Get().OnHoldActionProgressed = FFloatDelegate::CreateWeakLambda(Widget, [Widget, WeakCallback](float Value)
    {
        if (const auto Pinned = WeakCallback.Pin(); Pinned != nullptr)
        {
            Pinned->Invoke(Widget, Value);
        }
    });
}

void UBindUIActionArgsExtensions::BindOnHoldActionPressed(FBindUIActionArgsRef Args,
                                                          UWidget* Widget,
                                                          const FManagedDelegateHandle InOnHoldActionPressed)
{
    auto *Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    const auto Callback = MakeShared<TCSInputBindingCallback<>>(FGCHandle(InOnHoldActionPressed.Ptr, GCHandleType::StrongHandle));
    Subsystem->RegisterInputBindingCallback(Widget, FGuid::NewGuid(), Callback);
    TWeakPtr<TCSInputBindingCallback<>> WeakCallback = Callback;

    Args.Get().OnHoldActionPressed = FSimpleDelegate::CreateWeakLambda(Widget, [Widget, WeakCallback]
    {
        if (const auto Pinned = WeakCallback.Pin(); Pinned != nullptr)
        {
            Pinned->Invoke(Widget);
        }
    });
}

void UBindUIActionArgsExtensions::BindOnHoldActionReleased(FBindUIActionArgsRef Args,
                                                          UWidget* Widget,
                                                          const FManagedDelegateHandle InOnHoldActionReleased)
{
    auto *Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    const auto Callback = MakeShared<TCSInputBindingCallback<>>(FGCHandle(InOnHoldActionReleased.Ptr, GCHandleType::StrongHandle));
    Subsystem->RegisterInputBindingCallback(Widget, FGuid::NewGuid(), Callback);
    TWeakPtr<TCSInputBindingCallback<>> WeakCallback = Callback;

    Args.Get().OnHoldActionReleased = FSimpleDelegate::CreateWeakLambda(Widget, [Widget, WeakCallback]
    {
        if (const auto Pinned = WeakCallback.Pin(); Pinned != nullptr)
        {
            Pinned->Invoke(Widget);
        }
    });
}

FInputBindingCallbackRef UBindUIActionArgsExtensions::GetBoundDelegateData(const UWidget* Widget, const FGuid Id)
{
    auto *Subsystem = UGameplayStatics::GetGameInstance(Widget)->GetSubsystem<UCSCommonUISubsystem>();
    return Subsystem->GetInputBindingCallback(Widget, Id); 
}
