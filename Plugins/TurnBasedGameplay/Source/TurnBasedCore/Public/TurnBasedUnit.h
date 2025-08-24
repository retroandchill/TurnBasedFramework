// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "TurnBasedUnitComponent.h"
#include "UObject/Object.h"
#include "TurnBasedUnit.generated.h"

template <typename T, typename... A>
concept InitializableUnit = std::derived_from<T, UTurnBasedUnit> && requires(T* Unit, A&&... Args)
{
    Unit->Initialize(std::forward<A>(Args)...);
};

USTRUCT(BlueprintType, meta=(InternalType))
struct FManagedInitializerDelegate
{
    GENERATED_BODY()

    FGCHandleIntPtr ManagedDelegate;

    void Invoke(UTurnBasedUnit* Unit) const;
};


/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDCORE_API UTurnBasedUnit : public UObject
{
    GENERATED_BODY()

public:
    template <std::derived_from<UTurnBasedUnit> T, typename... A>
        requires InitializableUnit<T, A...>
    static T* Create(UObject* Outer, TSubclassOf<T> UnitClass, A&&... Args)
    {
        return Create<T>(Outer, UnitClass, [Args...](T& Unit)
        {
            Unit.Initialize(std::forward<A>(Args)...);
        });
    }

    template <std::derived_from<UTurnBasedUnit> T, std::invocable<T&> Functor>
    static T* Create(UObject* Outer, TSubclassOf<T> UnitClass, Functor&& Initialize)
    {
        auto Unit = NewObject<T>(Outer, UnitClass);
        Forward<Functor>(Initialize)(*Unit);
        Unit->InitializeComponents();
        Unit->PostInitializeComponents();
        Unit->K2_PostInitializeComponents();
        return Unit;
    }
    
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
    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent()
    {
        return RegisterNewComponent<T>(T::StaticClass());
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent(TSubclassOf<T> ComponentClass)
    {
        checkf(!ComponentCache.Contains(ComponentClass), TEXT("Component %s already registered"), *ComponentClass->GetName());

        auto NewComponent = NewObject<T>(this, ComponentClass);
        ComponentCache.Add(ComponentClass, NewComponent);
        return NewComponent;
    }

    virtual void PostInitializeComponents()
    {
        // No implementation here
    }

    UFUNCTION(BlueprintImplementableEvent, Category = "Components", meta = (ScriptName = "PostInitializeComponents"))
    void K2_PostInitializeComponents();

private:
    void InitializeComponents();
    
    UFUNCTION(meta = (DeterminesOutputType = "ComponentClass", DynamicOutputParam = "ReturnValue", ScriptMethod))
    UTurnBasedUnitComponent* RegisterNewComponentInternal(TSubclassOf<UTurnBasedUnitComponent> ComponentClass);

    UFUNCTION(meta = (DeterminesOutputType = "ComponentClass", DynamicOutputParam = "ReturnValue", ScriptMethod))
    static UTurnBasedUnit* Create(UObject* Outer, TSubclassOf<UTurnBasedUnit> ComponentClass,
        FManagedInitializerDelegate ManagedInitializer);
    
    UPROPERTY(EditAnywhere, Instanced, Category = "Components")
    TArray<TObjectPtr<UTurnBasedUnitComponent>> AdditionalComponents;

    mutable TMap<TSubclassOf<UTurnBasedUnitComponent>, TObjectPtr<UTurnBasedUnitComponent>> ComponentCache;
};
