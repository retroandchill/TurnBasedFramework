// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnitFactory.h"

#include "TurnBasedUnit.h"

UTurnBasedUnit* UTurnBasedUnitFactory::Create(UObject* Outer, const TSubclassOf<UTurnBasedUnit> UnitClass,
                                              const FInstancedStruct& InitializerHandle)
{
    auto Unit = NewObject<UTurnBasedUnit>(Outer, UnitClass);

    return Unit;
}
