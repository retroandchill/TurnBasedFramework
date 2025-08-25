// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "StructUtils/InstancedStruct.h"
#include "UObject/Object.h"
#include "TurnBasedUnitFactory.generated.h"

class UTurnBasedUnit;
class UTurnBasedUnitComponent;

/**
 * 
 */
UCLASS(Abstract, Blueprintable)
class TURNBASEDCORE_API UTurnBasedUnitFactory : public UObject
{
    GENERATED_BODY()
    
public:
    UTurnBasedUnit* Create(UObject* Outer, TSubclassOf<UTurnBasedUnit> UnitClass, const FInstancedStruct& InitializerHandle);

private:
    UPROPERTY(EditDefaultsOnly, Category = Components)
    FInstancedStruct Initializer;
    
    UPROPERTY(EditDefaultsOnly, Category = Components)
    TArray<TSubclassOf<UTurnBasedUnitComponent>> AdditionalComponents;
};
