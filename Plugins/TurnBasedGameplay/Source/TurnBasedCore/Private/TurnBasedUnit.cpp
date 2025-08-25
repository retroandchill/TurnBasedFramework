// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnit.h"

#include "Misc/DataValidation.h"

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

#if WITH_EDITOR
EDataValidationResult UTurnBasedUnit::IsDataValid(FDataValidationContext& Context) const
{
    TSet<TSubclassOf<UTurnBasedUnitComponent>> FoundComponents;
    for (auto [Property, Address] : TPropertyValueRange<FObjectProperty>(GetClass(), this, EPropertyValueIteratorFlags::NoRecursion)) 
    {
        if (FoundComponents.Contains(Property->PropertyClass))
        {
            Context.AddError(FText::Format(
                NSLOCTEXT("TurnBasedCore", "DuplicateProperty", "Property {0} is of type {1}, which is already used by another property"),
                FText::FromString(Property->GetName()),
                FText::FromString(Property->PropertyClass->GetName())));
        }
        
        if (const auto Component = Property->GetObjectPropertyValue(Address); Component == nullptr)
        {
            Context.AddError(FText::Format(NSLOCTEXT("TurnBasedCore", "NullComponentField", "Property {0} is set to null"), FText::FromString(Property->GetName())));
        }

        FoundComponents.Add(Property->PropertyClass);
    }

    for (auto &Component : AdditionalComponents)
    {
        if (Component == nullptr)
        {
            Context.AddError(NSLOCTEXT("TurnBasedCore", "NullComponentAdditionalProperty", "Found a null element in the Additional Components array"));
        }
        else
        {
            if (FoundComponents.Contains(Component->GetClass()))
            {
                Context.AddError(FText::Format(
                    NSLOCTEXT("TurnBasedCore", "DuplicateProperty", "There is already a component of type {0} on this actor"),
                    FText::FromString(Component->GetClass()->GetName())));
            }
        }
    }
    
    return UObject::IsDataValid(Context);
}
#endif

bool UTurnBasedUnit::RegisterNewComponentInternal(UTurnBasedUnitComponent* Component)
{
    if (ComponentCache.Contains(Component->GetClass()))
    {
        return false;
    }
    
    RegisterNewComponent(Component);
    return true;
}
