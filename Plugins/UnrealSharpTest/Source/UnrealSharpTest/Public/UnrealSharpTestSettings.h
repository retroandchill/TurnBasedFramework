// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Engine/DeveloperSettings.h"
#include "UnrealSharpTestSettings.generated.h"

/**
 * 
 */
UCLASS(Config=Engine, meta = (DisplayName = "UnrealSharp Test"), DefaultConfig)
class UNREALSHARPTEST_API UUnrealSharpTestSettings : public UDeveloperSettings
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintPure, BlueprintInternalUseOnly)
    const FString& GetSharedPrefix() const
    {
        return SharedPrefix;
    }
    
private:
    UPROPERTY(EditDefaultsOnly, BlueprintGetter=GetSharedPrefix, Category = "Test Information")
    FString SharedPrefix = TEXT("Managed");
};
