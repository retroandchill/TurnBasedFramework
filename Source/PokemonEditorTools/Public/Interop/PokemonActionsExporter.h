// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSBindsManager.h"
#include "UObject/Object.h"
#include "PokemonActionsExporter.generated.h"

struct FPokemonManagedActions;

/**
 * 
 */
UCLASS()
class POKEMONEDITORTOOLS_API UPokemonActionsExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static void SetActions(const FPokemonManagedActions& InActions);
};
