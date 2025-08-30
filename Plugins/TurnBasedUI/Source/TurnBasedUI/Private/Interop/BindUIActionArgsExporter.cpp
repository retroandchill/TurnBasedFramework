// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/BindUIActionArgsExporter.h"

#include "Input/CommonUIInputTypes.h"
#include "Utilities/ManagedDelegateCallback.h"

const FProperty* UBindUIActionArgsExporter::GetExemptInputTypesProperty()
{
    static FProperty* Property = FCommonInputTypeSet::StaticStruct()->FindPropertyByName(GET_MEMBER_NAME_CHECKED(FCommonInputTypeSet, ContainedSet));
    return Property;
}

void UBindUIActionArgsExporter::ConstructFromActionTag(FBindUIActionArgs& Args, FUIActionTag InActionTag,
    const FGCHandleIntPtr InOnExecuteAction)
{
    std::construct_at(&Args, InActionTag, FSimpleDelegate::CreateLambda(TManagedDelegateCallback(InOnExecuteAction)));
}

void UBindUIActionArgsExporter::ConstructFromRowHandle(FBindUIActionArgs& Args, UDataTable* DataTable, const FName RowName,
    const FGCHandleIntPtr InOnExecuteAction)
{
    FDataTableRowHandle Handle;
    Handle.DataTable = DataTable;
    Handle.RowName = RowName;
    
    std::construct_at(&Args, Handle, FSimpleDelegate::CreateLambda(TManagedDelegateCallback(InOnExecuteAction)));
}

void UBindUIActionArgsExporter::ConstructFromInputAction(FBindUIActionArgs& Args, const UInputAction* InInputAction,
    const FGCHandleIntPtr InOnExecuteAction)
{
    std::construct_at(&Args, InInputAction, FSimpleDelegate::CreateLambda(TManagedDelegateCallback(InOnExecuteAction)));
}

void UBindUIActionArgsExporter::Destruct(FBindUIActionArgs& Args)
{
    std::destroy_at(&Args);  
}

void UBindUIActionArgsExporter::BindNoArgsDelegate(FSimpleDelegate& Delegate, const FGCHandleIntPtr ManagedDelegate)
{
    Delegate.BindLambda(TManagedDelegateCallback(ManagedDelegate));
}

void UBindUIActionArgsExporter::BindFloatDelegate(TDelegate<void(float)>& Delegate, const FGCHandleIntPtr ManagedDelegate)
{
    Delegate.BindLambda(TManagedDelegateCallback<float>(ManagedDelegate));   
}

FName UBindUIActionArgsExporter::GetActionName(const FBindUIActionArgs& Args)
{
    return Args.GetActionName();
}

bool UBindUIActionArgsExporter::ActionHasHoldMappings(const FBindUIActionArgs& Args)
{
    return Args.ActionHasHoldMappings();
}
