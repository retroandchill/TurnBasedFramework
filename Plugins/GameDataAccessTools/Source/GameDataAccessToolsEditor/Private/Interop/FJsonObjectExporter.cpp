// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/FJsonObjectExporter.h"

void UFJsonObjectExporter::CreateJsonObject(TSharedPtr<FJsonObject>& JsonObject)
{
    JsonObject = MakeShared<FJsonObject>(); 
}

void UFJsonObjectExporter::SetField(const TSharedPtr<FJsonObject>& JsonObject, const char* FieldName,
                                    TSharedPtr<FJsonValue>& Value)
{
    // Create a local copy to ensure when the native code exits the reference counter has been decremented
    const auto LocalCopy = MoveTemp(Value);
    JsonObject->SetField(FieldName, LocalCopy);  
}

void UFJsonObjectExporter::CreateJsonIterator(const TSharedPtr<FJsonValue>& JsonObject, FJsonObjectIterator& Iterator)
{
    static_assert(sizeof(FJsonObjectIterator) <= 64);
    static_assert(std::is_trivially_destructible_v<FJsonObjectIterator>);
    std::construct_at(&Iterator, JsonObject->AsObject()->Values.CreateConstIterator()); 
}

bool UFJsonObjectExporter::AdvanceJsonIterator(FJsonObjectIterator& Iterator)
{
    if (!Iterator)
    {
        return false;
    }

    ++Iterator;
    return true;
}

bool UFJsonObjectExporter::IsValidJsonIterator(const FJsonObjectIterator& Iterator)
{
    return static_cast<bool>(Iterator);
}

void UFJsonObjectExporter::GetJsonIteratorValues(const FJsonObjectIterator& Iterator, const FString*& FieldName,
                                                 const TSharedPtr<FJsonValue>*& ObjectValue)
{
    auto& [Key, Value] = *Iterator;
    FieldName = &Key;
    ObjectValue = &Value; 
}
