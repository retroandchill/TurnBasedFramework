// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnit.h"

UTurnBasedUnit* UTurnBasedUnitComponent::GetOwningUnit() const
{
    return CastChecked<UTurnBasedUnit>(GetOuter());
}

void UTurnBasedUnitComponent::PostInitializeUnit()
{
    NativePostInitializeUnit();
    K2_PostInitializeUnit();  
}

// ReSharper disable once CppMemberFunctionMayBeConst
UTurnBasedUnitComponent* UTurnBasedUnit::RegisterNewComponent(UTurnBasedUnitComponent* Component)
{
    Components.Emplace(Component);
    return Component;
}

void UTurnBasedUnit::CreateComponents()
{
    NativeCreateComponents();
    K2_CreateComponents();   
}

UTurnBasedUnit* UTurnBasedUnit::CreateInternal(UObject* Outer, const TSubclassOf<UTurnBasedUnit> UnitClass)
{
    const auto NewComponent = NewObject<UTurnBasedUnit>(Outer, UnitClass);
    NewComponent->CreateComponents();
    return NewComponent;  
}
