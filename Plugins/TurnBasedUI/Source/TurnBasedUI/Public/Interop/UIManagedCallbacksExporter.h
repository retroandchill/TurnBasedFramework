// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "UIManagedCallbacksExporter.generated.h"

struct FUIManagedActions;
/**
 * 
 */
UCLASS()
class TURNBASEDUI_API UUIManagedCallbacksExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void SetActions(const FUIManagedActions& Actions);
};
