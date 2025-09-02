// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "Input/CommonUIInputTypes.h"
#include "CSBindUIActionArgs.generated.h"

USTRUCT()
struct FBindUIActionArgsRef
{
    GENERATED_BODY()

    FBindUIActionArgsRef() = default;
    explicit FBindUIActionArgsRef(FBindUIActionArgs& InRef) : Ref(&InRef) {}

    FBindUIActionArgs& Get()
    {
        check(Ref != nullptr);
        return *Ref;  
    }

    const FBindUIActionArgs& Get() const
    {
        check(Ref != nullptr);
        return *Ref;
    }

private:
    FBindUIActionArgs* Ref = nullptr;
};