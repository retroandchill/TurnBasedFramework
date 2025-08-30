// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedDelegate.h"
#include "Interop/UIManagedCallbacks.h"

/**
 * 
 */
template <typename... A>
class TManagedDelegateCallback
{
public:
    explicit TManagedDelegateCallback(const FGCHandleIntPtr ManagedDelegate) : Handle(ManagedDelegate)
    {
        
    }

    void operator()(A&&... Args) const
    {
        if constexpr (sizeof...(A) == 0)
        {
            FCSManagedCallbacks::ManagedCallbacks.InvokeDelegate(Handle.GetHandle());
        }
        else
        {
            FUIManagedCallbacks::Get().CallDelegate(Handle.GetHandle(), Forward<A>(Args)...);   
        }
    }

private:
    FSharedGCHandle Handle;
};