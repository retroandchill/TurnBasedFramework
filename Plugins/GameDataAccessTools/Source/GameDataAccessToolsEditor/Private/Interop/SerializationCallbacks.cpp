// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/SerializationCallbacks.h"


FSerializationCallbacks& FSerializationCallbacks::Get()
{
    static FSerializationCallbacks Instance;
    return Instance;   
}

void FSerializationCallbacks::SetActions(const FSerializationActions& InActions)
{
    this->Actions = InActions;
}

void FSerializationCallbacks::ForEachSerializationAction(const UClass* Class, const FSerializationAction& Action) const
{
    check(Actions.ForEachSerializationAction != nullptr);
    Actions.ForEachSerializationAction(Class, &Action);
}

FText FSerializationCallbacks::GetActionText(const FGCHandleIntPtr Handle) const
{
    FText Result;
    check(Actions.GetActionText != nullptr);
    Actions.GetActionText(Handle, &Result);
    return Result;
}
