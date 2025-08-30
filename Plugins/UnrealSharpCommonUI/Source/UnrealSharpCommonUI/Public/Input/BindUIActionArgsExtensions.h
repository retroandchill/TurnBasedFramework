// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonInputTypeEnum.h"
#include "Kismet/BlueprintFunctionLibrary.h"
#include "BindUIActionArgsExtensions.generated.h"

USTRUCT(meta = (SkipGlueGeneration))
struct FCommonInputTypeSet
{
    GENERATED_BODY()
    
    UPROPERTY()
    TSet<ECommonInputType> ContainedSet; 
};

/**
 * 
 */
UCLASS(meta = (InternalType))
class UNREALSHARPCOMMONUI_API UBindUIActionArgsExtensions : public UBlueprintFunctionLibrary
{
    GENERATED_BODY()

public:
    UFUNCTION(meta = (ScriptMethod))
    static int32 GetBindUIActionArgsSize();

    UFUNCTION(meta = (ScriptMethod))
    static int32 GetMemberOffset(FName MemberName);

private:
    static TMap<FName, int32> GetOffsets();
};
