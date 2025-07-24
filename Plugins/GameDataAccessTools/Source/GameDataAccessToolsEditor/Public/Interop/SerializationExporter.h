// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "SerializationCallbacks.h"
#include "UObject/Object.h"
#include "SerializationExporter.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API USerializationExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void AssignSerializationActions(const FSerializationActions& SerializationActions);

    UNREALSHARP_FUNCTION()
    static void OnSerializationAction(const FSerializationAction* Action, const FGCHandleIntPtr Handle);
};
