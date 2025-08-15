// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpAutomationTestBase.h"

#include "UnrealSharpTest.h"


void FCSharpAutomationTestBase::CollectCSharpTests(TArray<FString>& OutBeautifiedNames,
                                                   TArray<FString>& OutTestCommands)
{
    for (const auto& [Name, Handle] : FUnrealSharpTestModule::Get().GetTestHandles())
    {
        OutBeautifiedNames.Add(Name);
        OutTestCommands.Add(Name);   
    }
}
