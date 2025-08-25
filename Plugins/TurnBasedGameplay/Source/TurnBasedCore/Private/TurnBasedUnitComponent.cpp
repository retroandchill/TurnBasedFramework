// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnitComponent.h"

#include "TurnBasedUnit.h"

UTurnBasedUnit* UTurnBasedUnitComponent::GetOwningUnit() const
{
    return CastChecked<UTurnBasedUnit>(GetOuter());
}
