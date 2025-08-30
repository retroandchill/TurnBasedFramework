// Fill out your copyright notice in the Description page of Project Settings.


#include "CSCommonUISubsystem.h"

#include "Components/Widget.h"
#include "Input/CSInputBindingCallbacks.h"

void UCSCommonUISubsystem::Deinitialize()
{
    InputBindingCallbacks.Empty();
    GUObjectArray.RemoveUObjectDeleteListener(this);
}

void UCSCommonUISubsystem::RegisterInputBindingCallback(const UWidget* Widget,
                                                        const FGuid& Id,
                                                        const TSharedRef<FCSInputBindingCallback>& Callback)
{
    auto &WidgetCallbacks = InputBindingCallbacks.FindOrAdd(Widget->GetUniqueID());
    WidgetCallbacks.Add(Id, Callback);
}

FCSInputBindingCallback* UCSCommonUISubsystem::GetInputBindingCallback(const UWidget* Widget, const FGuid& Guid)
{
    const auto &WidgetCallbacks = InputBindingCallbacks.FindOrAdd(Widget->GetUniqueID());
    return WidgetCallbacks.FindRef(Guid).Get();
}


void UCSCommonUISubsystem::NotifyUObjectDeleted(const UObjectBase* Object, int32 Index)
{
    InputBindingCallbacks.Remove(Object->GetUniqueID());
}

void UCSCommonUISubsystem::OnUObjectArrayShutdown()
{
    GUObjectArray.RemoveUObjectDeleteListener(this);
}
