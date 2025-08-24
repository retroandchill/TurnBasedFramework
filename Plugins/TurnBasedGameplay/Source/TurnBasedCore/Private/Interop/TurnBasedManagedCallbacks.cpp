// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/TurnBasedManagedCallbacks.h"


FTurnBasedManagedCallbacks& FTurnBasedManagedCallbacks::Get()
{
    static FTurnBasedManagedCallbacks Instance;
    return Instance;   
}

void FTurnBasedManagedCallbacks::SetActions(const FTurnBasedManagedActions& InActions)
{
    Actions = InActions;
}

void FTurnBasedManagedCallbacks::ManagedInitialize(const FGCHandleIntPtr ActionPtr, UTurnBasedUnit* Unit) const
{
    Actions.OnUnitInitialized(ActionPtr, Unit);
}
