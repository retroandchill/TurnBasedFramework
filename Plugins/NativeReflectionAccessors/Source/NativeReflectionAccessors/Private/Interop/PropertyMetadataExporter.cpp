// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/PropertyMetadataExporter.h"

void UPropertyMetadataExporter::GetName(const FProperty* Property, FString* OutName)
{
    *OutName = Property->GetName();
}

FName UPropertyMetadataExporter::GetFName(const FProperty* Property)
{
    return Property->GetFName();
}

void UPropertyMetadataExporter::GetDisplayName(const FProperty* Property, FText* OutName)
{
    *OutName = Property->GetDisplayNameText();
}

void UPropertyMetadataExporter::GetToolTip(const FProperty* Property, FText* OutToolTip)
{
    *OutToolTip = Property->GetToolTipText();   
}

bool UPropertyMetadataExporter::IsNativeBool(const FBoolProperty* Property)
{
    return Property->IsNativeBool();   
}

uint8 UPropertyMetadataExporter::GetFieldMask(const FBoolProperty* Property)
{
    return Property->GetFieldMask();
}

UField* UPropertyMetadataExporter::GetWrappedType(const FProperty* Property)
{
    if (Property->IsA<FEnumProperty>())
    {
        return static_cast<const FEnumProperty*>(Property)->GetEnum();
    }
    
    if (Property->IsA<FStructProperty>())
    {
        return static_cast<const FStructProperty*>(Property)->Struct;   
    }

    if (Property->IsA<FClassProperty>())
    {
        return static_cast<const FClassProperty*>(Property)->MetaClass;
    }

    if (Property->IsA<FInterfaceProperty>())
    {
        return static_cast<const FInterfaceProperty*>(Property)->InterfaceClass;   
    }

    if (Property->IsA<FSoftClassProperty>())
    {
        return static_cast<const FSoftClassProperty*>(Property)->MetaClass;   
    }

    if (Property->IsA<FObjectPropertyBase>())
    {
        return static_cast<const FObjectPropertyBase*>(Property)->PropertyClass;   
    }

    return nullptr;
}
