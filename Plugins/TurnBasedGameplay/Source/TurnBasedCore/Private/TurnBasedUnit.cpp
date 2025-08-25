// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnit.h"

UTurnBasedUnit* UTurnBasedUnitComponent::GetOwningUnit() const
{
    return CastChecked<UTurnBasedUnit>(GetOuter());
}

bool UTurnBasedUnitComponent::TryGetSiblingComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
    UTurnBasedUnitComponent*& OutComponent) const
{
    OutComponent = GetSiblingComponent<UTurnBasedUnitComponent>(ComponentClass);
    return OutComponent != nullptr;  
}

bool UTurnBasedUnit::TryGetComponent(const TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
                                     UTurnBasedUnitComponent*& OutComponent) const
{
    OutComponent = GetComponent(ComponentClass);
    return OutComponent != nullptr;   
}

// ReSharper disable once CppMemberFunctionMayBeConst
void UTurnBasedUnit::RegisterNewComponent(UTurnBasedUnitComponent* Component)
{
    checkf(!ComponentCache.Contains(Component->GetClass()), TEXT("Component %s already registered"), *Component->GetClass()->GetName());
    ComponentCache.Add(Component->GetClass(), Component);
}

bool UTurnBasedUnit::RegisterNewComponentInternal(UTurnBasedUnitComponent* Component)
{
    if (ComponentCache.Contains(Component->GetClass()))
    {
        return false;
    }
    
    RegisterNewComponent(Component);
    return true;
}
