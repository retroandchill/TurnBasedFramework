// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/ManagedTestHandle.h"

#include "Interop/ManagedTestingCallbacks.h"


FString FManagedTestHandle::GetFullyQualifiedName() const
{
    return FManagedTestingCallbacks::Get().GetFullyQualifiedName(Handle.GetHandle());   
}
