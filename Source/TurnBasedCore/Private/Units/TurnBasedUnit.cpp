// Fill out your copyright notice in the Description page of Project Settings.


#include "Units/TurnBasedUnit.h"

#include "Units/TurnBasedUnitComponent.h"

UTurnBasedUnitComponent* UTurnBasedUnit::GetComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass) const {
    for (auto Component : Components) {
        if (Component->IsA(ComponentClass)) {
            return Component;
        }
    }

    return nullptr;
}

bool UTurnBasedUnit::TryGetComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
    UTurnBasedUnitComponent*& Component) const {
    Component = GetComponent(ComponentClass);
    return Component != nullptr;
}
