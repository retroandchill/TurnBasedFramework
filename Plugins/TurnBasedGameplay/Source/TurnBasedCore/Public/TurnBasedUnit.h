// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "TurnBasedUnitComponent.h"
#include "UObject/Object.h"
#include "TurnBasedUnit.generated.h"

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

        for (auto& Component : AdditionalComponents)
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

#if WITH_EDITOR
    EDataValidationResult IsDataValid(FDataValidationContext& Context) const override;
#endif

private:
    UFUNCTION(meta = (ScriptMethod))
    bool RegisterNewComponentInternal(UTurnBasedUnitComponent* Component);
    
    UPROPERTY(EditAnywhere, Instanced, Category = "Components")
    TArray<TObjectPtr<UTurnBasedUnitComponent>> AdditionalComponents;

    mutable TMap<TSubclassOf<UTurnBasedUnitComponent>, TObjectPtr<UTurnBasedUnitComponent>> ComponentCache;
};
