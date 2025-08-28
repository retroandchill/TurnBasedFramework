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

    void PostInitializeUnit();
    
protected:
    virtual void NativePostInitializeUnit() { }
    
    UFUNCTION(BlueprintImplementableEvent, DisplayName = "Post Initialize Unit", Category = "Components", meta = (ScriptName = "PostInitializeUnit"))
    void K2_PostInitializeUnit();
};

/**
 * 
 */
UCLASS(Abstract)
class TURNBASEDCORE_API UTurnBasedUnit : public UObject
{
    GENERATED_BODY()

public:
    template <std::derived_from<UTurnBasedUnit> T>
    static T* Create(UObject* Outer)
    {
        return Create<T>(Outer, [] (T*) { });  
    }

    template <std::derived_from<UTurnBasedUnit> T>
    static T* Create(UObject* Outer, std::invocable<T*> auto ConstructorFunc)
    {
        auto NewComponent = NewObject<T>(Outer);
        NewComponent->InitializeComponents();
        ConstructorFunc(NewComponent);
        for (auto &Component : NewComponent->Components)
        {
            Component->PostInitializeUnit();
        }
        return NewComponent;   
    }

    template <std::derived_from<UTurnBasedUnit> T = UTurnBasedUnit>
    static T* Create(UObject* Outer, TSubclassOf<T> UnitClass)
    {
        return Create<T>(Outer, UnitClass, [] (T*) { }); 
    }

    template <std::derived_from<UTurnBasedUnit> T = UTurnBasedUnit>
    static T* Create(UObject* Outer, TSubclassOf<T> UnitClass, std::invocable<T*> auto ConstructorFunc)
    {
        auto NewComponent = NewObject<T>(Outer, UnitClass);
        NewComponent->InitializeComponents();
        ConstructorFunc(NewComponent);
        for (auto &Component : NewComponent->Components)
        {
            Component->PostInitializeUnit();
        }
        return NewComponent;   
    }
    
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

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent()
    {
        return RegisterNewComponent<T>([] (T*) { });
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent(const TSubclassOf<UTurnBasedUnitComponent> ComponentClass)
    {
        return RegisterNewComponent<T>(ComponentClass, [] (T*) { });
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent(std::invocable<T*> auto ConstructorFunc)
    {
        T* NewComponent = NewObject<T>(this);
        ConstructorFunc(NewComponent);
        return static_cast<T*>(RegisterNewComponent());   
    }

    template <std::derived_from<UTurnBasedUnitComponent> T = UTurnBasedUnitComponent>
    T* RegisterNewComponent(const TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
                            std::invocable<T*> auto ConstructorFunc)
    {
        auto NewComponent = NewObject<T>(this, ComponentClass);
        ConstructorFunc(NewComponent);
        return static_cast<T*>(RegisterNewComponent());   
    }

    UFUNCTION(BlueprintCallable, Category = "Components", meta = (DeterminesOutputType = Component, DynamicOutputParam = ReturnValue))
    UTurnBasedUnitComponent* RegisterNewComponent(UTurnBasedUnitComponent* Component);

protected:
    virtual void NativeCreateComponents() { }
    
    UFUNCTION(BlueprintImplementableEvent, DisplayName = "Create Components", Category = "Components", meta = (ScriptName = "CreateComponents"))
    void K2_CreateComponents();
    
private:
    void CreateComponents();
    
    UFUNCTION(meta = (ScriptMethod, DeterminesOutputType = UnitClass, DynamicOutputParam = ReturnValue))
    static UTurnBasedUnit* CreateInternal(UObject* Outer, TSubclassOf<UTurnBasedUnit> UnitClass);
    
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
