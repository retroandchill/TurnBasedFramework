// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Utils/expected.hpp"

struct FPokemonManagedActions
{
    using FPopulateEvolutions = bool(__stdcall*)(const FProperty*, const FScriptMap*, FString* Error);

    FPopulateEvolutions PopulateEvolutions = nullptr;
};

class POKEMONEDITORTOOLS_API FPokemonManagedCallbacks
{
    FPokemonManagedCallbacks() = default;
    ~FPokemonManagedCallbacks() = default;

public:
    static FPokemonManagedCallbacks& Get();

    void SetActions(const FPokemonManagedActions& InActions);

    tl::expected<void, FString> PopulateEvolutions(const FProperty* Property, const FScriptMap& ScriptMap) const;

private:
    FPokemonManagedActions Actions;
};