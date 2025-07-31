// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/FJsonObjectConverterExporter.h"

#include "JsonObjectConverter.h"
#include "Misc/DefaultValueHelper.h"
#include "UObject/PropertyIterator.h"

bool UFJsonObjectConverterExporter::SerializeObjectToJson(const UObject* TargetObject, TSharedPtr<FJsonValue>& JsonValue)
{
    static const FName JsonOrder = "JsonOrder";
    auto JsonObject = MakeShared<FJsonObject>();
    if (!FJsonObjectConverter::UStructToJsonObject(TargetObject->GetClass(), TargetObject, JsonObject,
        0, 0, nullptr, EJsonObjectConversionFlags::WriteTextAsComplexString))
    {
        return false;
    }

    TMap<FName, TPair<int32, bool>> SortedProperties;
    int32 PropertyCount = 0;
    for (const auto Prop : TFieldRange<FProperty>(TargetObject->GetClass()))
    {
        if (Prop->HasMetaData(JsonOrder))
        {
            auto OrderProperty = Prop->GetMetaData(JsonOrder);
            if (int32 Order; FDefaultValueHelper::ParseInt(OrderProperty, Order))
            {
                SortedProperties.Emplace(Prop->GetFName(), {Order, true});
                continue;
            }
        }

        SortedProperties.Emplace(Prop->GetFName(), {PropertyCount, false});
        PropertyCount++;
    }

    JsonObject->Values.KeySort([&SortedProperties](const FString& A, const FString& B)
    {
        auto [OrderA, IsExplicitOrderA] = SortedProperties.FindChecked(FName(A));
        auto [OrderB, IsExplicitOrderB] = SortedProperties.FindChecked(FName(B));
        
        // We want to put anything with an explicit order first.
        return IsExplicitOrderA == IsExplicitOrderB ? OrderA < OrderB : IsExplicitOrderA;        
    });
    
    JsonValue = MakeShared<FJsonValueObject>(MoveTemp(JsonObject));
    return true;  
}

bool UFJsonObjectConverterExporter::DeserializeJsonToObject(TSharedPtr<FJsonValue>& JsonValue,
                                                            UObject* TargetObject, FText* FailureReason)
{
    const auto LocalCopy = MoveTemp(JsonValue);
    TSharedPtr<FJsonObject>* InnerObject;
    if (!ensure(LocalCopy->TryGetObject(InnerObject)))
    {
        *FailureReason = NSLOCTEXT("GameDataEntry", "InvalidJson", "Invalid Json");
        return false;
    }
    
    return FJsonObjectConverter::JsonObjectToUStruct(InnerObject->ToSharedRef(), TargetObject->GetClass(),
        TargetObject, 0, 0, false, FailureReason);
}
