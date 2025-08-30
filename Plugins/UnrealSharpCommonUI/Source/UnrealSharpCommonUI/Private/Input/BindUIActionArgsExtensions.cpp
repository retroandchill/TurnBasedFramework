// Fill out your copyright notice in the Description page of Project Settings.


#include "Input/BindUIActionArgsExtensions.h"

#include "Input/CommonUIInputTypes.h"

#define BINDING_ARGS(Name) { GET_MEMBER_NAME_CHECKED(FBindUIActionArgs, Name), offsetof(FBindUIActionArgs, Name) }

int32 UBindUIActionArgsExtensions::GetBindUIActionArgsSize()
{
    return sizeof(FBindUIActionArgs);
}

int32 UBindUIActionArgsExtensions::GetMemberOffset(const FName MemberName)
{
    static const auto Offsets = GetOffsets();
    return Offsets.FindRef(MemberName);   
}

TMap<FName, int32> UBindUIActionArgsExtensions::GetOffsets()
{
    return {
        BINDING_ARGS(ActionTag),
        BINDING_ARGS(bConsumeInput),
        BINDING_ARGS(bDisplayInActionBar),
        BINDING_ARGS(bForceHold),
        BINDING_ARGS(bIsPersistent),
        BINDING_ARGS(InputAction),
        BINDING_ARGS(InputMode),
        BINDING_ARGS(KeyEvent),
        BINDING_ARGS(LegacyActionTableRow),
        BINDING_ARGS(OnExecuteAction),
        BINDING_ARGS(OnHoldActionPressed),
        BINDING_ARGS(OnHoldActionProgressed),
        BINDING_ARGS(OnHoldActionReleased),
        BINDING_ARGS(OverrideDisplayName)
    };
}
