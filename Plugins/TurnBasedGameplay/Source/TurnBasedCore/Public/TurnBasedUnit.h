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
    UTurnBasedUnitComponent* GetSiblingComponent() const
    {
        return GetSiblingComponent<T>(T::StaticClass());
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    UTurnBasedUnitComponent* GetSiblingComponent(TSubclassOf<T> ComponentClass) const;

    UFUNCTION(BlueprintCallable, DisplayName = "Get Sibling Component", Category = "Components", meta = (ScriptName = "TryGetSiblingComponent", DeterminesOutputType = "ComponentClass", DynamicOutputParam = "OutComponent"))
    bool TryGetSiblingComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass, UTurnBasedUnitComponent*& OutComponent) const;
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
        return GetComponent<T>(T::StaticClass());
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* GetComponent(TSubclassOf<T> ComponentClass) const
    {
        auto Cached = ComponentCache.Find(ComponentClass);
        if (Cached != nullptr)
        {
            return CastChecked<T>(*Cached);
        }

        for (auto& Component : Components)
        {
            if (Component->IsA(ComponentClass))
            {
                ComponentCache.Add(ComponentClass, Component);
                return CastChecked<T>(Component);
            }
        }

        return nullptr;
    }

    UFUNCTION(BlueprintCallable, DisplayName = "Get Component", Category = "Components", meta = (ScriptName = "TryGetComponent", DeterminesOutputType = "ComponentClass", DynamicOutputParam = "OutComponent"))
    bool TryGetComponent(TSubclassOf<UTurnBasedUnitComponent> ComponentClass, UTurnBasedUnitComponent*& OutComponent) const;

protected:
    void RegisterNewComponent(UTurnBasedUnitComponent* Component);

    virtual void PostInitializeComponents()
    {
        // No implementation here
    }

private:
    UFUNCTION(meta = (ScriptMethod))
    bool RegisterNewComponentInternal(UTurnBasedUnitComponent* Component);
    
    UPROPERTY()
    TArray<TObjectPtr<UTurnBasedUnitComponent>> Components;

    mutable TMap<TSubclassOf<UTurnBasedUnitComponent>, TObjectPtr<UTurnBasedUnitComponent>> ComponentCache;
};

template <std::derived_from<UTurnBasedUnitComponent> T>
UTurnBasedUnitComponent* UTurnBasedUnitComponent::GetSiblingComponent(TSubclassOf<T> ComponentClass) const
{
    return GetOwningUnit()->GetComponent<T>(ComponentClass);   
}
