// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"

class GAMEDATAACCESSTOOLSEDITOR_API FGameDataEntrySerializer
{
public:
    explicit FGameDataEntrySerializer(const FGCHandleIntPtr Ptr) : Handle(Ptr) {}

    FText GetFormatName() const;
    
private:
    FScopedGCHandle Handle;

};
