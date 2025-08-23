// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "expected.hpp"

struct FPokemonManagedActions
{
    using FPopulateEvolutions = bool(__stdcall*)(const FProperty*, const FScriptMap*, FString* Error);
    using FGetEvolutionConditionClass = bool(__stdcall*)(const FGameplayTag, TSubclassOf<UObject>*, FString* Error);

    FPopulateEvolutions PopulateEvolutions = nullptr;
    FGetEvolutionConditionClass GetEvolutionConditionClass = nullptr;
};

class POKEMONEDITORTOOLS_API FPokemonManagedCallbacks
{
    FPokemonManagedCallbacks() = default;
    ~FPokemonManagedCallbacks() = default;

public:
    static FPokemonManagedCallbacks& Get();

    void SetActions(const FPokemonManagedActions& InActions);

    tl::expected<void, FString> PopulateEvolutions(const FProperty* Property, const FScriptMap& ScriptMap) const;
    tl::expected<UClass*, FString> GetEvolutionConditionClass(const FGameplayTag Tag) const;

private:
    FPokemonManagedActions Actions;
};