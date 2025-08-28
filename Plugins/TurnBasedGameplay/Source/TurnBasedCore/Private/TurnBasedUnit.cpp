// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnit.h"

UTurnBasedUnit* UTurnBasedUnitComponent::GetOwningUnit() const
{
    return CastChecked<UTurnBasedUnit>(GetOuter());
}

// ReSharper disable once CppMemberFunctionMayBeConst
void UTurnBasedUnit::RegisterNewComponent(UTurnBasedUnitComponent* Component)
{
    Components.Add(Component);
}
