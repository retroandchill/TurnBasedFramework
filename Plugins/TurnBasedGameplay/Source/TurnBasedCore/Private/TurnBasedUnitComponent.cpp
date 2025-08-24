// Fill out your copyright notice in the Description page of Project Settings.


#include "TurnBasedUnitComponent.h"

void UTurnBasedUnitComponent::InitializeComponent(UTurnBasedUnit* Unit)
{
    Owner = Unit;
    NativeInitialize();
    K2_Initialize();   
}

void UTurnBasedUnitComponent::NativeInitialize()
{
    // No baseline implementation
}
