// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

using FSerializationAction = TFunctionRef<void(const FGCHandleIntPtr)>;

struct FSerializationActions
{
    using FForEachSerializationAction = void(__stdcall*)(const UClass*, const FSerializationAction*);
    using FGetActionText = void(__stdcall*)(FGCHandleIntPtr, FText*);

    FForEachSerializationAction ForEachSerializationAction = nullptr;
    FGetActionText GetActionText = nullptr;
};

class FSerializationCallbacks
{
    FSerializationCallbacks() = default;
    ~FSerializationCallbacks() = default;

public:
    static FSerializationCallbacks& Get();

    void SetActions(const FSerializationActions& InActions);
    void ForEachSerializationAction(const UClass* Class, const FSerializationAction& Action) const;
    FText GetActionText(const FGCHandleIntPtr Handle) const;

private:
    FSerializationActions Actions;
};