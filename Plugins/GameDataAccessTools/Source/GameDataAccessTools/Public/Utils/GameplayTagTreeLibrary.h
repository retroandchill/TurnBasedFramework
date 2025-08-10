// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "GameplayTagTreeLibrary.generated.h"

/**
 * 
 */
UCLASS()
class GAMEDATAACCESSTOOLS_API UGameplayTagTreeLibrary : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    /**
     * Retrieves all child gameplay tags of the specified parent gameplay tag.
     *
     * This method interacts with the GameplayTagsManager to return a container
     * of gameplay tags that are direct or indirect children of the provided tag.
     *
     * @param Tag The parent gameplay tag for which child tags should be retrieved.
     * @return A container of child gameplay tags associated with the given parent tag.
     */
    UFUNCTION(BlueprintCallable, Category = "GameplayTags", meta=(ExtensionMethod))
    static FGameplayTagContainer GetGameplayTagChildren(const FGameplayTag Tag);
};
