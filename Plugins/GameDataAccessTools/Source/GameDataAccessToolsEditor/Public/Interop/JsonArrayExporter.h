// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "JsonArrayExporter.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API UJsonArrayExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static int32 GetSize(const TArray<TSharedPtr<FJsonValue>>& Values);

    UNREALSHARP_FUNCTION()
    static const TSharedPtr<FJsonValue>& GetAtIndex(const TArray<TSharedPtr<FJsonValue>>& Values, int32 Index);

    UNREALSHARP_FUNCTION()
    static void AddToArray(TArray<TSharedPtr<FJsonValue>>& Values, TSharedPtr<FJsonValue>& Value);
};
