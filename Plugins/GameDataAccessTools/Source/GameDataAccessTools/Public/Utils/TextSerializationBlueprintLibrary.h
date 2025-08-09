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
    static FText CreateLocalizedText(const FString& Namespace, const FString& Key, const FString& DefaultValue);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static bool TryGetNamespace(const FText& Text, FString& OutNamespace);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static bool TryGetKey(const FText& Text, FString& OutKey);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSubclassOf<UObject> GetClassFromPath(const FString& Path);
    
    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSoftObjectPtr<UObject> GetSoftObjectPtrFromPath(const FString& Path);

    UFUNCTION(BlueprintPure, Category = "Serialization|Text", meta=(ExtensionMethod))
    static TSoftClassPtr<UObject> GetSoftClassPtrFromPath(const FString& Path);
};
