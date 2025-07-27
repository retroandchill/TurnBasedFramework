// Fill out your copyright notice in the Description page of Project Settings.


#include "Handles/DataHandlePropertyIdentifier.h"

#include "TypeGenerator/CSScriptStruct.h"
#include "TypeGenerator/Register/TypeInfo/CSStructInfo.h"

bool FDataHandlePropertyIdentifier::IsPropertyTypeCustomized(const IPropertyHandle& PropertyHandle) const
{
    const auto Property = CastFieldChecked<FStructProperty>(PropertyHandle.GetProperty());
    const auto ManagedStruct = Cast<UCSScriptStruct>(Property->Struct);
    if (ManagedStruct == nullptr)
    {
        return false;
    }

    
    return ManagedStruct->GetTypeInfo()->TypeMetaData->MetaData.Contains("GameDataEntryHandle")
        && ManagedStruct->FindPropertyByName("RowName") != nullptr;
}
