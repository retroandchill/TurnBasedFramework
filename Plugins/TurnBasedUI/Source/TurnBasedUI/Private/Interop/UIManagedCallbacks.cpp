// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/UIManagedCallbacks.h"


FUIManagedCallbacks& FUIManagedCallbacks::Get()
{
    static FUIManagedCallbacks Instance;
    return Instance;   
}

void FUIManagedCallbacks::SetActions(const FUIManagedActions& InActions)
{
    Actions = InActions;  
}

void FUIManagedCallbacks::CallDelegate(const FGCHandleIntPtr ManagedDelegate, const float Value) const
{
    check(Actions.CallFloatDelegate != nullptr);
    Actions.CallFloatDelegate(ManagedDelegate, Value);   
}
