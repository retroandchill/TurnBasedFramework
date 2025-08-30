// Fill out your copyright notice in the Description page of Project Settings.


#include "CSCommonUISubsystem.h"

#include "Components/Widget.h"
#include "Input/CSInputBindingCallbacks.h"

void UCSCommonUISubsystem::Deinitialize()
{
    CallbackBindings.Empty();
    GUObjectArray.RemoveUObjectDeleteListener(this);
}

void UCSCommonUISubsystem::BindInputActionCallbacks(UWidget* InObject, const FGCHandle& OnExecuteAction,
    const FGCHandle& OnHoldActionPressed, const FGCHandle& OnHoldActionReleased,
    const FGCHandle& OnHoldActionProgressed)
{
    const uint32 UniqueId = InObject->GetUniqueID();
    auto& Callbacks = CallbackBindings.FindOrAdd(UniqueId);
    Callbacks.Add(MakeShared<FCSInputBindingCallbacks>(InObject, OnExecuteAction, OnHoldActionPressed, OnHoldActionReleased, OnHoldActionProgressed));
}

void UCSCommonUISubsystem::NotifyUObjectDeleted(const UObjectBase* Object, int32 Index)
{
    CallbackBindings.Remove(Object->GetUniqueID());
}

void UCSCommonUISubsystem::OnUObjectArrayShutdown()
{
    GUObjectArray.RemoveUObjectDeleteListener(this);
}
