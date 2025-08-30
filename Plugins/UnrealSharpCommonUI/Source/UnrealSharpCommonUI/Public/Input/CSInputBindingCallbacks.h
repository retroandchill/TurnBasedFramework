// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedDelegate.h"

#include "CSInputBindingCallbacks.generated.h"

USTRUCT()
struct FManagedDelegateHandle
{
    GENERATED_BODY()

    FGCHandleIntPtr Ptr;
};

class FCSInputBindingCallback
{
public:
    UE_NONCOPYABLE(FCSInputBindingCallback)

    explicit FCSInputBindingCallback(const FGCHandle& Handle) : Delegate(Handle) {}
    
    ~FCSInputBindingCallback()
    {
        Delegate.Dispose();
    }

protected:
    void InvokeInternal(UObject* WorldContext)
    {
        Delegate.Invoke(WorldContext, false);
    }

private:
    FCSManagedDelegate Delegate;
};

template <typename... T>
    requires ((std::is_default_constructible_v<T>) && ...)
class TCSInputBindingCallback : public FCSInputBindingCallback
{
public:
    using FCSInputBindingCallback::FCSInputBindingCallback;

    template <typename... A>
        requires std::constructible_from<TTuple<T...>, A...> 
    void Invoke(UObject* WorldContext, A&&... Args)
    {
        LastParams = TTuple<T...>(Forward<A>(Args)...);
        InvokeInternal(WorldContext);   
    }

private:
    TTuple<T...> LastParams;
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
    void Invoke(UObject* WorldContext, A&& Args)
    {
        LastParam = Forward<A>(Args);
        InvokeInternal(WorldContext);  
    }

private:
    T LastParam;
};

USTRUCT()
struct FInputBindingCallbackRef
{
    GENERATED_BODY()

    FInputBindingCallbackRef() = default;
    explicit(false) FInputBindingCallbackRef(FCSInputBindingCallback& Handle) : Ptr(&Handle) {}
    explicit(false) FInputBindingCallbackRef(FCSInputBindingCallback* Handle) : Ptr(Handle) {}

    FCSInputBindingCallback& Get() const
    {
        check(Ptr != nullptr);
        return *Ptr;
    }
    
private:
    FCSInputBindingCallback* Ptr;
};