// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/JsonArrayExporter.h"

int32 UJsonArrayExporter::GetSize(const TArray<TSharedPtr<FJsonValue>>& Values)
{
    return Values.Num();   
}

const TSharedPtr<FJsonValue>& UJsonArrayExporter::GetAtIndex(const TArray<TSharedPtr<FJsonValue>>& Values, const int32 Index)
{
    return Values[Index];  
}

void UJsonArrayExporter::AddToArray(TArray<TSharedPtr<FJsonValue>>& Values, TSharedPtr<FJsonValue>& Value)
{
    // Move it into the array to remove the responsibility for disposing the reference counter from C#
    Values.Emplace(MoveTemp(Value));
}
