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
