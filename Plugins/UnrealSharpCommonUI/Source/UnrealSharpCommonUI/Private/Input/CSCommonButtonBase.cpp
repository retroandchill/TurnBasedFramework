// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/CSCommonButtonBase.h"

void UCSCommonButtonBase::UpdateInputActionWidget()
{
    Super::UpdateInputActionWidget();
    K2_UpdateInputActionWidget(); 
}
