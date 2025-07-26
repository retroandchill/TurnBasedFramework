// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/FJsonObjectConverterExporter.h"

#include "JsonObjectConverter.h"

bool UFJsonObjectConverterExporter::SerializeObjectToJson(const UObject* TargetObject, TSharedPtr<FJsonValue>& JsonValue)
{
    auto JsonObject = MakeShared<FJsonObject>();
    if (!FJsonObjectConverter::UStructToJsonObject(TargetObject->GetClass(), TargetObject, JsonObject))
    {
        return false;
    }
    
    JsonValue = MakeShared<FJsonValueObject>(MoveTemp(JsonObject));
    return true;  
}

bool UFJsonObjectConverterExporter::DeserializeJsonToObject(TSharedPtr<FJsonValue>& JsonValue,
                                                            UObject* TargetObject, FText* FailureReason)
{
    auto LocalCopy = MoveTemp(JsonValue);
    TSharedPtr<FJsonObject>* InnerObject;
    if (!ensure(JsonValue->TryGetObject(InnerObject)))
    {
        *FailureReason = NSLOCTEXT("GameDataEntry", "InvalidJson", "Invalid Json");
        return false;
    }
    
    return FJsonObjectConverter::JsonObjectToUStruct(InnerObject->ToSharedRef(), TargetObject->GetClass(),
        TargetObject, 0, 0, false, FailureReason);
}
