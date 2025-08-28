// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "TurnBasedUnit.generated.h"

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

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    UTurnBasedUnitComponent* GetSiblingComponent() const;

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    UTurnBasedUnitComponent* GetSiblingComponent(TSubclassOf<T> ComponentClass) const;
};

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDCORE_API UTurnBasedUnit : public UObject
{
    GENERATED_BODY()

public:
    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* GetComponent() const
    {
        for (auto &Component : Components)
        {
            if (auto *FoundComponent = Cast<T>(Component.Get()))
            {
                return FoundComponent;
            }
        }

        return nullptr;   
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* GetComponent(TSubclassOf<T> ComponentClass) const
    {
        for (auto &Component : Components)
        {
            if (Component->IsA(ComponentClass))
            {
                return static_cast<T*>(Component.Get());
            }
        }

        return nullptr;
    }

    UFUNCTION(BlueprintCallable, Category = "Components")
    void RegisterNewComponent(UTurnBasedUnitComponent* Component);

private:
    UPROPERTY()
    TArray<TObjectPtr<UTurnBasedUnitComponent>> Components;
};

template <std::derived_from<UTurnBasedUnitComponent> T>
UTurnBasedUnitComponent* UTurnBasedUnitComponent::GetSiblingComponent() const
{
    return GetOwningUnit()->GetComponent<T>(); 
}

template <std::derived_from<UTurnBasedUnitComponent> T>
UTurnBasedUnitComponent* UTurnBasedUnitComponent::GetSiblingComponent(TSubclassOf<T> ComponentClass) const
{
    return GetOwningUnit()->GetComponent<T>(ComponentClass);   
}
