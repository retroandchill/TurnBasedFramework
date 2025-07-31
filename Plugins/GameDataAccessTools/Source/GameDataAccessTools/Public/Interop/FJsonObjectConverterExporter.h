// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "FJsonObjectConverterExporter.generated.h"


/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UFJsonObjectConverterExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static bool SerializeObjectToJson(const UObject* TargetObject, TSharedPtr<FJsonValue>& JsonValue);
    
    UNREALSHARP_FUNCTION()
    static bool DeserializeJsonToObject(TSharedPtr<FJsonValue>& JsonValue, UObject* TargetObject, FText* FailureReason);
};
