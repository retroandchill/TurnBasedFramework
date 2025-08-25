// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "TurnBasedUnitComponent.generated.h"

class UTurnBasedUnit;
/**
 * 
 */
UCLASS(Abstract, EditInlineNew)
class TURNBASEDCORE_API UTurnBasedUnitComponent : public UObject
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, Category = "Component")
    UTurnBasedUnit* GetOwningUnit() const;
    
};
