// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedDelegate.h"
#include "Components/Widget.h"

class FCSInputBindingCallback
{
public:
    explicit FCSInputBindingCallback(const FGCHandle& InCallback) : Callback(InCallback) {}

    FCSInputBindingCallback(const FCSInputBindingCallback&) = delete;
    FCSInputBindingCallback(FCSInputBindingCallback&&) = delete;

    ~FCSInputBindingCallback()
    {
        Callback.Dispose();
    }

    FCSInputBindingCallback& operator=(const FCSInputBindingCallback&) = delete;
    FCSInputBindingCallback& operator=(FCSInputBindingCallback&&) = delete;

    void Invoke(UObject* WorldContext)
    {
        Callback.Invoke(WorldContext, false);
    } 

private:
    FCSManagedDelegate Callback;
};

/**
 * 
 */
class UNREALSHARPCOMMONUI_API FCSInputBindingCallbacks
{
public:
    FCSInputBindingCallbacks(UWidget* InObject, const FGCHandle& OnExecuteAction, const FGCHandle& OnHoldActionPressed,
        const FGCHandle& OnHoldActionReleased, const FGCHandle& OnHoldActionProgressed) : Object(InObject),
        OnExecuteAction(OnExecuteAction), OnHoldActionPressed(OnHoldActionPressed), OnHoldActionReleased(OnHoldActionReleased),
        OnHoldActionProgressed(OnHoldActionProgressed)
    {
        
    }

    void InvokeOnExecuteAction()
    {
        OnExecuteAction.Invoke(Object.Get());
    }

    void InvokeOnHoldActionPressed()
    {
        OnHoldActionPressed.Invoke(Object.Get());
    }

    void InvokeOnHoldActionReleased()
    {
        OnHoldActionReleased.Invoke(Object.Get());
    }

    void InvokeOnHoldActionProgressed(const float InProgress)
    {
        Progress = InProgress;
        OnHoldActionProgressed.Invoke(Object.Get());
    }

    float GetProgress() const { return Progress; }

private:
    TWeakObjectPtr<UWidget> Object;
    FCSInputBindingCallback OnExecuteAction;
    FCSInputBindingCallback OnHoldActionPressed;
    FCSInputBindingCallback OnHoldActionReleased;
    FCSInputBindingCallback OnHoldActionProgressed;
    float Progress = 0.f;
};
