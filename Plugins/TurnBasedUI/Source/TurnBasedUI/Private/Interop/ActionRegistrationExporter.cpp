// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/ActionRegistrationExporter.h"

#include "CommonUserWidget.h"

void UActionRegistrationExporter::GetActionBindings(const UCommonUserWidget* Widget, const FUIActionBindingHandle*& Bindings,
                                                    int32& NumBindings)
{
    auto &AllBindings = Widget->GetActionBindings();
    Bindings = AllBindings.GetData();
    NumBindings = AllBindings.Num();  
}

void UActionRegistrationExporter::RegisterActionBinding(UCommonUserWidget* Widget, const FBindUIActionArgs& Args,
    FUIActionBindingHandle* Handle)
{
    *Handle = Widget->RegisterUIActionBinding(Args);   
}

void UActionRegistrationExporter::AddActionBinding(UCommonUserWidget* Widget, FUIActionBindingHandle* Handle)
{
    Widget->AddActionBinding(MoveTemp(*Handle));
}

void UActionRegistrationExporter::RemoveActionBinding(UCommonUserWidget* Widget, FUIActionBindingHandle* Handle)
{
    Widget->RemoveActionBinding(MoveTemp(*Handle));
}

void UActionRegistrationExporter::DestructHandle(FUIActionBindingHandle* Handle)
{
    std::destroy_at(Handle); 
}
