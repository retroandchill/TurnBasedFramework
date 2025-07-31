// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "TextSerializationBlueprintLibrary.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UTextSerializationBlueprintLibrary : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod, ScriptMethod))
    static FText FromLocalizedString(const FString& LocalizedString);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod, ScriptMethod))
    static FString ToLocalizedString(const FText& Text);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod, ScriptMethod))
    static TSubclassOf<UObject> GetClassFromPath(const FString& Path);
};
