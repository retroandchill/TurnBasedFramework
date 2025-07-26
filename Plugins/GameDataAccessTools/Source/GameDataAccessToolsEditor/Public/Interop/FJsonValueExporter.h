// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "FJsonValueExporter.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API UFJsonValueExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void CreateJsonNull(TSharedPtr<FJsonValue>& JsonValue);

    UNREALSHARP_FUNCTION()
    static void CreateJsonBool(TSharedPtr<FJsonValue>& JsonValue, bool bValue);

    UNREALSHARP_FUNCTION()
    static void CreateJsonNumber(TSharedPtr<FJsonValue>& JsonValue, double Value);

    UNREALSHARP_FUNCTION()
    static void CreateJsonString(TSharedPtr<FJsonValue>& JsonValue, const char* Value);

    UNREALSHARP_FUNCTION()
    static void CreateJsonArray(TSharedPtr<FJsonValue>& JsonValue, TArray<TSharedPtr<FJsonValue>>& Values);
    
    UNREALSHARP_FUNCTION()
    static void CreateJsonObject(TSharedPtr<FJsonValue>& JsonValue, TSharedPtr<FJsonObject>& Object);
};
