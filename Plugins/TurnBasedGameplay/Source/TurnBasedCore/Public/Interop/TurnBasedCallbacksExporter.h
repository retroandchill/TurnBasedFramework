// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UnrealSharpBinds/Public/CSBindsManager.h"
#include "UObject/Object.h"
#include "TurnBasedCallbacksExporter.generated.h"

struct FTurnBasedManagedActions;
/**
 * 
 */
UCLASS()
class TURNBASEDCORE_API UTurnBasedCallbacksExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void SetActions(const FTurnBasedManagedActions& Actions);
};
