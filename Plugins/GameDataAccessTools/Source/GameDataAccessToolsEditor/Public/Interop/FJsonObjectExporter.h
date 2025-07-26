// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "FJsonObjectExporter.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API UFJsonObjectExporter : public UObject
{
    GENERATED_BODY()

public:
    using FJsonObjectIterator = TMap<FString, TSharedPtr<FJsonValue>>::TConstIterator;
    
    UNREALSHARP_FUNCTION()
    static void CreateJsonObject(TSharedPtr<FJsonObject>& JsonObject);
    
    UNREALSHARP_FUNCTION()
    static void SetField(const TSharedPtr<FJsonObject>& JsonObject, const char* FieldName, TSharedPtr<FJsonValue>& Value);

    UNREALSHARP_FUNCTION()
    static void CreateJsonIterator(const TSharedPtr<FJsonObject>& JsonObject, FJsonObjectIterator& Iterator);

    UNREALSHARP_FUNCTION()
    static bool AdvanceJsonIterator(FJsonObjectIterator& Iterator);

    UNREALSHARP_FUNCTION()
    static bool IsValidJsonIterator(const FJsonObjectIterator& Iterator);

    UNREALSHARP_FUNCTION()
    static void GetJsonIteratorValues(const FJsonObjectIterator& Iterator, const FString*& FieldName, const TSharedPtr<FJsonValue>*& ObjectValue);
};
