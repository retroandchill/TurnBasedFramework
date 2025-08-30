// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "CommonInputTypeEnum.h"
#include "CSBindsManager.h"
#include "CSManagedGCHandle.h"
#include "UITag.h"
#include "Input/CommonUIInputTypes.h"
#include "UObject/Object.h"
#include "BindUIActionArgsExporter.generated.h"

struct FBindUIActionArgs;
class UInputAction;

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
UCLASS()
class TURNBASEDUI_API UBindUIActionArgsExporter : public UObject
{
    GENERATED_BODY()

public:
    UNREALSHARP_FUNCTION()
    static const FProperty* GetExemptInputTypesProperty();
    
    UNREALSHARP_FUNCTION()
    static void ConstructFromActionTag(FBindUIActionArgs& Args, FUIActionTag InActionTag, FGCHandleIntPtr InOnExecuteAction);
    
    UNREALSHARP_FUNCTION()
    static void ConstructFromRowHandle(FBindUIActionArgs& Args, UDataTable* DataTable, FName RowName, FGCHandleIntPtr InOnExecuteAction);
    
    UNREALSHARP_FUNCTION()
    static void ConstructFromInputAction(FBindUIActionArgs& Args, const UInputAction* InInputAction, FGCHandleIntPtr InOnExecuteAction);

    UNREALSHARP_FUNCTION()
    static void Destruct(FBindUIActionArgs& Args);
    
    UNREALSHARP_FUNCTION()
    static void BindNoArgsDelegate(FSimpleDelegate& Delegate, FGCHandleIntPtr ManagedDelegate);

    UNREALSHARP_FUNCTION()
    static void BindFloatDelegate(TDelegate<void(float)>& Delegate, FGCHandleIntPtr ManagedDelegate);

    UNREALSHARP_FUNCTION()
    static FName GetActionName(const FBindUIActionArgs& Args);

    UNREALSHARP_FUNCTION()
    static bool ActionHasHoldMappings(const FBindUIActionArgs& Args);
};
