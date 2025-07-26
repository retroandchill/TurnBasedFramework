// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/FJsonValueExporter.h"

void UFJsonValueExporter::CreateJsonNull(TSharedPtr<FJsonValue>& JsonValue)
{
    JsonValue = MakeShared<FJsonValueNull>();
}

void UFJsonValueExporter::CreateJsonBool(TSharedPtr<FJsonValue>& JsonValue, bool bValue)
{
    JsonValue = MakeShared<FJsonValueBoolean>(bValue);
}

void UFJsonValueExporter::CreateJsonNumber(TSharedPtr<FJsonValue>& JsonValue, double Value)
{
    JsonValue = MakeShared<FJsonValueNumber>(Value);
}

void UFJsonValueExporter::CreateJsonString(TSharedPtr<FJsonValue>& JsonValue, const char* Value)
{
    JsonValue = MakeShared<FJsonValueString>(Value);
}

void UFJsonValueExporter::CreateJsonArray(TSharedPtr<FJsonValue>& JsonValue, TArray<TSharedPtr<FJsonValue>>& Values)
{
    // Use the move constructor to avoid copies and to remove the responsibility for management from C#
    JsonValue = MakeShared<FJsonValueArray>(MoveTemp(Values));
}

void UFJsonValueExporter::CreateJsonObject(TSharedPtr<FJsonValue>& JsonValue, TSharedPtr<FJsonObject>& Object)
{
    // Use the move constructor to avoid copies and to remove the responsibility for management from C#
    JsonValue = MakeShared<FJsonValueObject>(MoveTemp(Object));  
}

EJson UFJsonValueExporter::GetJsonType(const TSharedPtr<FJsonValue>& JsonValue)
{
    return JsonValue->Type; 
}

void UFJsonValueExporter::DestroyJsonValue(TSharedPtr<FJsonValue>& JsonValue)
{
    std::destroy_at(&JsonValue);
}

bool UFJsonValueExporter::GetJsonBool(const TSharedPtr<FJsonValue>& JsonValue)
{
    return JsonValue->AsBool();
}

double UFJsonValueExporter::GetJsonNumber(const TSharedPtr<FJsonValue>& JsonValue)
{
    return JsonValue->AsNumber();
}

void UFJsonValueExporter::GetJsonString(const TSharedPtr<FJsonValue>& JsonValue, FString& OutString)
{
    JsonValue->TryGetString(OutString);
}

void UFJsonValueExporter::GetJsonArray(const TSharedPtr<FJsonValue>& JsonValue,
    const TArray<TSharedPtr<FJsonValue>>*& Values)
{
    JsonValue->TryGetArray(Values); 
}

void UFJsonValueExporter::GetJsonObject(const TSharedPtr<FJsonValue>& JsonValue,
    const TSharedPtr<FJsonObject>*& Object)
{
    JsonValue->TryGetObject(Object);
}