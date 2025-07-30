// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CSManagedGCHandle.h"
#include "Utils/expected.hpp"

class UGameDataRepository;

class GAMEDATAACCESSTOOLSEDITOR_API FGameDataEntrySerializer
{
public:
    explicit FGameDataEntrySerializer(const FGCHandleIntPtr Ptr) : Handle(Ptr) {}

    FText GetFormatName() const;

    FString GetFileExtensionText() const;

    tl::expected<FString, FString> SerializeData(const UGameDataRepository* Repository) const;

    tl::expected<TArray<UObject*>, FString> DeserializeData(
        const FString& Data, const UGameDataRepository* Repository) const;
    
private:
    FScopedGCHandle Handle;

};
