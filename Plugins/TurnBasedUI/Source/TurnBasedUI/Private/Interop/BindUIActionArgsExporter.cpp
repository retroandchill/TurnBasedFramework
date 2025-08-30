// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/BindUIActionArgsExporter.h"

#include "Bindings/CSBindUIActionCallbacksBase.h"
#include "Input/CommonUIInputTypes.h"

const FProperty* UBindUIActionArgsExporter::GetExemptInputTypesProperty()
{
    static FProperty* Property = FCommonInputTypeSet::StaticStruct()->FindPropertyByName(GET_MEMBER_NAME_CHECKED(FCommonInputTypeSet, ContainedSet));
    return Property;
}

void UBindUIActionArgsExporter::ConstructFromActionTag(FBindUIActionArgs& Args, FUIActionTag InActionTag,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    std::construct_at(&Args, InActionTag, FSimpleDelegate::CreateUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnExecuteAction));
}

void UBindUIActionArgsExporter::ConstructFromRowHandle(FBindUIActionArgs& Args, UDataTable* DataTable, const FName RowName,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    FDataTableRowHandle Handle;
    Handle.DataTable = DataTable;
    Handle.RowName = RowName;
    
    std::construct_at(&Args, Handle, FSimpleDelegate::CreateUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnExecuteAction));
}

void UBindUIActionArgsExporter::ConstructFromInputAction(FBindUIActionArgs& Args, const UInputAction* InInputAction,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    std::construct_at(&Args, InInputAction, FSimpleDelegate::CreateUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnExecuteAction));
}

void UBindUIActionArgsExporter::Destruct(FBindUIActionArgs& Args)
{
    std::destroy_at(&Args);  
}

void UBindUIActionArgsExporter::BindOnHoldActionProgressed(FBindUIActionArgs::FOnHoldActionProgressed& Delegate,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    Delegate.BindUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnHoldActionProgressed);
}

void UBindUIActionArgsExporter::BindOnHoldActionPressed(FBindUIActionArgs::FOnHoldActionPressed& Delegate,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    Delegate.BindUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnHoldActionPressed);
}

void UBindUIActionArgsExporter::BindOnHoldActionReleased(FBindUIActionArgs::FOnHoldActionReleased& Delegate,
    UCSBindUIActionCallbacksBase* ActionsBinding)
{
    Delegate.BindUObject(ActionsBinding, &UCSBindUIActionCallbacksBase::InvokeOnHoldActionReleased);
}

FName UBindUIActionArgsExporter::GetActionName(const FBindUIActionArgs& Args)
{
    return Args.GetActionName();
}

bool UBindUIActionArgsExporter::ActionHasHoldMappings(const FBindUIActionArgs& Args)
{
    return Args.ActionHasHoldMappings();
}
