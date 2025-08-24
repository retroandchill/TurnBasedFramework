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
    void InitializeComponent(UTurnBasedUnit* Unit);

    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    UTurnBasedUnit* GetOwner() const
    {
        return Owner;
    }

protected:
    virtual void NativeInitialize();

    UFUNCTION(BlueprintImplementableEvent, Category = "Lifecycle", meta = (ScriptName = "Initialize"))
    void K2_Initialize();

private:
    UPROPERTY(BlueprintGetter = GetOwner, Category = "Ownership")
    TObjectPtr<UTurnBasedUnit> Owner;
};
