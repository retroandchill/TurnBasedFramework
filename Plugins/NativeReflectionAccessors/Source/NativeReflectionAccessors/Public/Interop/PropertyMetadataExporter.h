// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "PropertyMetadataExporter.generated.h"

/**
 * 
 */
UCLASS()
class NATIVEREFLECTIONACCESSORS_API UPropertyMetadataExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void GetName(const FProperty* Property, FString* OutName);

    UNREALSHARP_FUNCTION()
    static FName GetFName(const FProperty* Property);

    UNREALSHARP_FUNCTION()
    static void GetDisplayName(const FProperty* Property, FText* OutName);

    UNREALSHARP_FUNCTION()
    static void GetToolTip(const FProperty* Property, FText* OutToolTip);

    UNREALSHARP_FUNCTION()
    static bool IsNativeBool(const FBoolProperty* Property);

    UNREALSHARP_FUNCTION()
    static uint8 GetFieldMask(const FBoolProperty* Property);
};
