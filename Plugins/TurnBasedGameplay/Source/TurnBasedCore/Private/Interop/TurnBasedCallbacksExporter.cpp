// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/TurnBasedCallbacksExporter.h"

#include "Interop/TurnBasedManagedCallbacks.h"

void UTurnBasedCallbacksExporter::SetActions(const FTurnBasedManagedActions& Actions)
{
    FTurnBasedManagedCallbacks::Get().SetActions(Actions);
}
