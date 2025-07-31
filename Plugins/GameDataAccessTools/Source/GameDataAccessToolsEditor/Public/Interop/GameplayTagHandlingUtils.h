// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "GameplayTagHandlingUtils.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLSEDITOR_API UGameplayTagHandlingUtils : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintCallable, Category = "GameplayTags")
    static bool TryAddGameplayTagToIni(FName TagSource, const FString& TagName, FString& Error);
};
