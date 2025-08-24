// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

class UTurnBasedUnit;

struct FTurnBasedManagedActions
{
    using FInvokeManagedInitializer = void(__stdcall*)(FGCHandleIntPtr, UTurnBasedUnit*);

    FInvokeManagedInitializer OnUnitInitialized;
};

/**
 * 
 */
class TURNBASEDCORE_API FTurnBasedManagedCallbacks
{
    FTurnBasedManagedCallbacks() = default;
    ~FTurnBasedManagedCallbacks() = default;

public:
    static FTurnBasedManagedCallbacks& Get();

    void SetActions(const FTurnBasedManagedActions& InActions);
    void ManagedInitialize(FGCHandleIntPtr ActionPtr, UTurnBasedUnit* Unit) const;

private:
    FTurnBasedManagedActions Actions;
};
