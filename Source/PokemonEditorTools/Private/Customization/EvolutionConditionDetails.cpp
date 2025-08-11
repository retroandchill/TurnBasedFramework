// Fill out your copyright notice in the Description page of Project Settings.


#include "Customization/EvolutionConditionDetails.h"

#include "DetailWidgetRow.h"
#include "IDetailChildrenBuilder.h"
#include "Interop/PokemonManagedCallbacks.h"
#include "TypeGenerator/CSScriptStruct.h"


struct FGameplayTag;

bool FEvolutionConditionPropertyIdentifier::IsPropertyTypeCustomized(const IPropertyHandle& PropertyHandle) const
{
    static const FName EvolutionConditionName = TEXT("EvolutionCondition");
    const auto Property = CastFieldChecked<FStructProperty>(PropertyHandle.GetProperty());
    const auto ManagedStruct = Cast<UCSScriptStruct>(Property->Struct);
    if (ManagedStruct == nullptr)
    {
        return false;
    }

    
    return ManagedStruct->GetFName() == EvolutionConditionName;
}

TSharedRef<IPropertyTypeCustomization> FEvolutionConditionDetails::MakeInstance()
{
    return MakeShared<FEvolutionConditionDetails>();
}

void FEvolutionConditionDetails::CustomizeHeader(const TSharedRef<IPropertyHandle> PropertyHandle,
    FDetailWidgetRow& HeaderRow, IPropertyTypeCustomizationUtils& CustomizationUtils)
{
    // clang-format off
    HeaderRow.NameContent()
        [
            PropertyHandle->CreatePropertyNameWidget()
        ]
        .ValueContent()
        [
            PropertyHandle->CreatePropertyValueWidget()
        ];
    // clang-format on
}

void FEvolutionConditionDetails::CustomizeChildren(const TSharedRef<IPropertyHandle> PropertyHandle,
    IDetailChildrenBuilder& ChildBuilder, IPropertyTypeCustomizationUtils& CustomizationUtils)
{
    const auto StructProperty = CastFieldChecked<FStructProperty>(PropertyHandle->GetProperty());
    const UScriptStruct* ScriptStruct = StructProperty->Struct;

    for (auto CurrentProperty = ScriptStruct->PropertyLink; CurrentProperty != nullptr; CurrentProperty = CastField<FProperty>(CurrentProperty->Next))
    {
        if (CurrentProperty->GetFName() == TEXT("Method"))
        {
            auto MethodHandle = PropertyHandle->GetChildHandle(CurrentProperty->GetFName()).ToSharedRef();
            
            // Set up the callback for when the value changes
            MethodHandle->SetOnPropertyValueChanged(FSimpleDelegate::CreateStatic(&FEvolutionConditionDetails::OnMethodChanged, PropertyHandle, MethodHandle));
            
            ChildBuilder.AddProperty(MethodHandle);
            continue;
        }
        
        // Skip the 'Data' property as we want to display its properties inline
        if (CurrentProperty->GetFName() != TEXT("Data"))
        {
            // Add all other properties normally
            const auto ChildHandle = PropertyHandle->GetChildHandle(CurrentProperty->GetFName()).ToSharedRef();
            ChildBuilder.AddProperty(ChildHandle);
            continue;
        }
        
        // Get the Data property handle
        auto DataHandle = PropertyHandle->GetChildHandle(CurrentProperty->GetFName());
        if (DataHandle == nullptr) continue;
            
        // Get the UObject value
        UObject* DataObject = nullptr;
        DataHandle->GetValue(DataObject);
                
        if (DataObject == nullptr)
        {
            continue;
        }
                
        // If we have a valid object, add its properties inline
        for (TFieldIterator<FProperty> PropIt(DataObject->GetClass()); PropIt; ++PropIt)
        {
            const auto ChildPropHandle = DataHandle->GetChildHandle(PropIt->GetFName()).ToSharedRef();
            ChildBuilder.AddProperty(ChildPropHandle);
        }
    }
}

void FEvolutionConditionDetails::OnMethodChanged(TSharedRef<IPropertyHandle> ParentProperty,
    TSharedRef<IPropertyHandle> MethodProperty)
{
    void* RawValue;
    MethodProperty->GetValueData(RawValue);
    const auto &Tag = *static_cast<FGameplayTag*>(RawValue);

    auto CreatedObject = FPokemonManagedCallbacks::Get().GetEvolutionConditionClass(Tag);

    if (!CreatedObject.has_value())
    {
        UE_LOG(LogTemp, Error, TEXT("Failed to create evolution condition object for tag %s\n%s"), *Tag.ToString(), *CreatedObject.error());
        return;
    }

    const auto DataProperty = ParentProperty->GetChildHandle(TEXT("Data"));
    check(DataProperty != nullptr);

    DataProperty->NotifyPreChange();
    TArray<void*> Objects;
    DataProperty->AccessRawData(Objects);
    const auto TargetClass = CreatedObject.value();
    for (const auto Object : Objects)
    {
        auto &AsObjectPointer = *static_cast<UObject**>(Object);
        AsObjectPointer = TargetClass != nullptr ? NewObject<UObject>(GetTransientPackage(), TargetClass) : nullptr;
    }
    DataProperty->NotifyPostChange(EPropertyChangeType::ValueSet);
}
