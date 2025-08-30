// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedDelegate.h"
#include "Components/Widget.h"

class FCSInputBindingCallback
{
public:
    explicit FCSInputBindingCallback(const FGCHandle& InCallback) : Callback(InCallback), bIsValid(!InCallback.IsNull()) {}

    FCSInputBindingCallback(const FCSInputBindingCallback&) = delete;
    FCSInputBindingCallback(FCSInputBindingCallback&&) = delete;

    ~FCSInputBindingCallback()
    {
        Callback.Dispose();
    }

    FCSInputBindingCallback& operator=(const FCSInputBindingCallback&) = delete;
    FCSInputBindingCallback& operator=(FCSInputBindingCallback&&) = delete;

    bool IsBound() const { return bIsValid; }

protected:
    void InvokeInternal(UObject* WorldContext)
    {
        Callback.Invoke(WorldContext, false);
    }

private:
    FCSManagedDelegate Callback;
    bool bIsValid;
};

template <typename... T>
class TCSInputBindingCallback : public FCSInputBindingCallback
{
public:
    using FCSInputBindingCallback::FCSInputBindingCallback;

    template <typename... A>
        requires std::constructible_from<TTuple<T...>, A...>
    void Invoke(UObject* WorldContext)
    {
        Args = TTuple<T...>(Forward<A>(Args)...);
        InvokeInternal(WorldContext);
    }
    
private:
    TTuple<T...> Args;
};

template <>
class TCSInputBindingCallback<> : public FCSInputBindingCallback
{
public:
    using FCSInputBindingCallback::FCSInputBindingCallback;
    
    void Invoke(UObject* WorldContext)
    {
        InvokeInternal(WorldContext);
    }
};

template <typename T>
class TCSInputBindingCallback<T> : public FCSInputBindingCallback
{
public:
    using FCSInputBindingCallback::FCSInputBindingCallback;

    template <std::convertible_to<T> A>
    void Invoke(UObject* WorldContext, A &&InArg)
    {
        Arg = Forward<A>(InArg);
        InvokeInternal(WorldContext);
    }

private:
    T Arg;
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

    bool IsOnHoldActionPressedBound() const
    {
        return OnHoldActionPressed.IsBound();
    }

    void InvokeOnHoldActionReleased()
    {
        OnHoldActionReleased.Invoke(Object.Get());
    }

    bool IsOnHoldActionReleasedBound() const
    {
        return OnHoldActionReleased.IsBound();
    }

    void InvokeOnHoldActionProgressed(const float InProgress)
    {
        OnHoldActionProgressed.Invoke(Object.Get(), InProgress);
    }

    bool IsOnHoldActionProgressedBound() const
    {
        return OnHoldActionProgressed.IsBound();
    }

private:
    TWeakObjectPtr<UWidget> Object;
    TCSInputBindingCallback<> OnExecuteAction;
    TCSInputBindingCallback<> OnHoldActionPressed;
    TCSInputBindingCallback<> OnHoldActionReleased;
    TCSInputBindingCallback<float> OnHoldActionProgressed;
};
