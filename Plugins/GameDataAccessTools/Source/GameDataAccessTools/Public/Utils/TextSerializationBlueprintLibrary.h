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
    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static FText FromLocalizedString(const FString& LocalizedString);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static FString ToLocalizedString(const FText& Text);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSubclassOf<UObject> GetClassFromPath(const FString& Path);
    
    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSoftObjectPtr<UObject> GetSoftObjectPtrFromPath(const FString& Path);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSoftClassPtr<UObject> GetSoftClassPtrFromPath(const FString& Path);
};
