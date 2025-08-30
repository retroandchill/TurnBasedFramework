// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "UObject/Object.h"
#include "Input/CommonUIInputTypes.h"
#include "CSBindUIActionArgs.generated.h"

USTRUCT()
struct FSimpleDelegateRef
{
    GENERATED_BODY()

    FSimpleDelegateRef() = default;
    explicit FSimpleDelegateRef(FSimpleDelegate& InDelegate) : Delegate(&InDelegate) {}

    FSimpleDelegate& Get()
    {
        check(Delegate != nullptr);
        return *Delegate;   
    }
    
    FSimpleDelegate& Get() const
    {
        check(Delegate != nullptr);
        return *Delegate;
    }

private:
    FSimpleDelegate* Delegate = nullptr;
};

DECLARE_DELEGATE_OneParam(FFloatDelegate, float);

USTRUCT()
struct FFloatDelegateRef
{
    GENERATED_BODY()

    FFloatDelegateRef() = default;
    explicit FFloatDelegateRef(FFloatDelegate& InDelegate) : Delegate(&InDelegate) {}

    FFloatDelegate& Get()
    {
        check(Delegate != nullptr);
        return *Delegate;   
    }
    
    FFloatDelegate& Get() const
    {
        check(Delegate != nullptr);
        return *Delegate;
    }

private:
    FFloatDelegate* Delegate = nullptr;
};

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