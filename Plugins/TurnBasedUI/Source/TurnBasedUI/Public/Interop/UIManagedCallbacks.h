// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

struct FUIManagedActions
{
    using FCallFloatDelegate = void(__stdcall*)(FGCHandleIntPtr, float);

    FCallFloatDelegate CallFloatDelegate = nullptr;
};

/**
 * 
 */
class TURNBASEDUI_API FUIManagedCallbacks
{
    FUIManagedCallbacks() = default;
    ~FUIManagedCallbacks() = default;

public:
    static FUIManagedCallbacks& Get();

    void SetActions(const FUIManagedActions& InActions);

    void CallDelegate(FGCHandleIntPtr ManagedDelegate, float Value) const;
    
private:
    FUIManagedActions Actions;
};
