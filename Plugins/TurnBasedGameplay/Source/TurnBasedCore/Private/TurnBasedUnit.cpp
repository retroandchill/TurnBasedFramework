// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnit.h"

#include "Interop/TurnBasedManagedCallbacks.h"

void FManagedInitializerDelegate::Invoke(UTurnBasedUnit* Unit) const
{
    FTurnBasedManagedCallbacks::Get().ManagedInitialize(ManagedDelegate, Unit);
}

bool UTurnBasedUnit::TryGetComponent(const TSubclassOf<UTurnBasedUnitComponent> ComponentClass,
                                     UTurnBasedUnitComponent*& OutComponent) const
{
    OutComponent = GetComponent(ComponentClass);
    return OutComponent != nullptr;   
}

void UTurnBasedUnit::InitializeComponents()
{
    for (auto &Component : AdditionalComponents)
    {
        ComponentCache.Add(Component->GetClass(), Component);
    }

    for (auto &[Class, Component] : ComponentCache)
    {
        Component->InitializeComponent(this);
    }
}

UTurnBasedUnitComponent* UTurnBasedUnit::RegisterNewComponentInternal(const TSubclassOf<UTurnBasedUnitComponent> ComponentClass)
{
    if (ComponentCache.Contains(ComponentClass))
    {
        return nullptr;
    }
    
    return RegisterNewComponent(ComponentClass);
}

UTurnBasedUnit* UTurnBasedUnit::Create(UObject* Outer, const TSubclassOf<UTurnBasedUnit> ComponentClass,
    const FManagedInitializerDelegate ManagedInitializer)
{
    FScopedGCHandle Scope(ManagedInitializer.ManagedDelegate);
    return Create(Outer, ComponentClass, [ManagedInitializer](UTurnBasedUnit& Unit)
    {
        ManagedInitializer.Invoke(&Unit);
    });
}
