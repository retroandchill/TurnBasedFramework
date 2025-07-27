// Fill out your copyright notice in the Description page of Project Settings.


#include "Units/DummyDataClass.h"

TArray<FName> UDummyDataClass::GetOptions()
{
    return {
        "Option1",
        "Option2",
        "Option3"
    };
}
