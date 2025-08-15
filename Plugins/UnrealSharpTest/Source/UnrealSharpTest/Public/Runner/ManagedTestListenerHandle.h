// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

/**
 * 
 */
class UNREALSHARPTEST_API FManagedTestListenerHandle
{
public:
    explicit FManagedTestListenerHandle(const FGCHandleIntPtr InHandle) : Handle(InHandle)
    {
    }

    FGCHandleIntPtr GetRawHandle() const
    {
        return Handle.GetHandle();   
    }
    
private:
    FSharedGCHandle Handle;
};
