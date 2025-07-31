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
class GAMEDATAACCESSTOOLS_API UFJsonValueExporter : public UObject
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

    UNREALSHARP_FUNCTION()
    static EJson GetJsonType(const TSharedPtr<FJsonValue>& JsonValue);

    UNREALSHARP_FUNCTION()
    static void DestroyJsonValue(TSharedPtr<FJsonValue>& JsonValue);

    UNREALSHARP_FUNCTION()
    static bool GetJsonBool(const TSharedPtr<FJsonValue>& JsonValue);
    
    UNREALSHARP_FUNCTION()
    static double GetJsonNumber(const TSharedPtr<FJsonValue>& JsonValue);

    UNREALSHARP_FUNCTION()
    static void GetJsonString(const TSharedPtr<FJsonValue>& JsonValue, FString& OutString);
    
    UNREALSHARP_FUNCTION()
    static void GetJsonArray(const TSharedPtr<FJsonValue>& JsonValue, const TArray<TSharedPtr<FJsonValue>>*& Values);

    UNREALSHARP_FUNCTION()
    static void GetJsonObject(const TSharedPtr<FJsonValue>& JsonValue, const TSharedPtr<FJsonObject>*& Object);
};
