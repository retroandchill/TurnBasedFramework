// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpAutomationTest.h"


EAutomationTestFlags FCSharpAutomationTest::GetTestFlags() const
{
    return EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter;
}

FString FCSharpAutomationTest::GetBeautifiedTestName() const
{
    return ManagedTestCase.FullyQualifiedName;
}

uint32 FCSharpAutomationTest::GetRequiredDeviceNum() const
{
    return 1;
}

void FCSharpAutomationTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
    OutBeautifiedNames.Add("");
    OutTestCommands.Add("");
}

bool FCSharpAutomationTest::RunTest(const FString& Parameters)
{
    return true;
}
