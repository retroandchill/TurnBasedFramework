// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "UnitAttribute.generated.h"

namespace TurnBased
{
}


USTRUCT(BlueprintType)
struct FBoolUnitAttribute
{
    GENERATED_BODY()

    UPROPERTY()
    uint8 BaseValue : 1 = false;

    UPROPERTY()
    uint8 CurrentValue : 1 = false;

    FBoolUnitAttribute() = default;

    explicit(false) FBoolUnitAttribute(bool Value) : BaseValue(Value), CurrentValue(Value)
    {
    }
};

USTRUCT(BlueprintType)
struct FIntUnitAttribute
{
    GENERATED_BODY()

    UPROPERTY()
    int32 BaseValue;

    UPROPERTY()
    int32 CurrentValue;

    FIntUnitAttribute() = default;

    explicit FIntUnitAttribute(int32 Value) : BaseValue(Value), CurrentValue(Value)
    {
    }
};

USTRUCT(BlueprintType)
struct FFloatUnitAttribute
{
    GENERATED_BODY()

    UPROPERTY()
    float BaseValue = 0.f;

    UPROPERTY()
    float CurrentValue = 0.f;

    FFloatUnitAttribute()
    {
    }

    explicit FFloatUnitAttribute(float Value) : BaseValue(Value), CurrentValue(Value)
    {
    }
};
