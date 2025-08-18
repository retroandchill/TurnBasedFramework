// Fill out your copyright notice in the Description page of Project Settings.


#include "Runner/CSharpAutomationTest.h"

#include "Runner/CSharpTestLatentCommand.h"


FString FCSharpAutomationTest::GetTestSourceFileName() const
{
    return ManagedTestCase.TestCase.CodeFilePath;
}

int32 FCSharpAutomationTest::GetTestSourceFileLine() const
{
    return ManagedTestCase.TestCase.LineNumber;
}

EAutomationTestFlags FCSharpAutomationTest::GetTestFlags() const
{
    return EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter;
}

FString FCSharpAutomationTest::GetBeautifiedTestName() const
{
    return ManagedTestCase.TestCase.FullyQualifiedName;
}

uint32 FCSharpAutomationTest::GetRequiredDeviceNum() const
{
    return 1;
}

void FCSharpAutomationTest::LogInfo(FStringView Message, FStringView SourceFile, int32 LineNumber)
{
    AddInfo(FString(Message), 0, true);
    OverrideTargetFile(SourceFile, LineNumber);
}

void FCSharpAutomationTest::LogWarning(FStringView Message, FStringView SourceFile, int32 LineNumber)
{
    AddWarning(FString(Message));
    OverrideTargetFile(SourceFile, LineNumber);
}

void FCSharpAutomationTest::LogError(FStringView Message, FStringView SourceFile, int32 LineNumber)
{
    AddError(FString(Message));
    OverrideTargetFile(SourceFile, LineNumber);
}

void FCSharpAutomationTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
    OutBeautifiedNames.Add("");
    OutTestCommands.Add("");
}

bool FCSharpAutomationTest::RunTest(const FString& Parameters)
{
    ADD_LATENT_AUTOMATION_COMMAND(FCSharpTestLatentCommand(AsShared(), ManagedTestCase.ManagedTestCase.GetHandle()));
    return true;
}

void FCSharpAutomationTest::OverrideTargetFile(FStringView SourceFile, const int32 LineNumber)
{
    auto &MostRecentEntry = const_cast<FAutomationExecutionEntry&>(ExecutionInfo.GetEntries().Last());
    MostRecentEntry.Filename = SourceFile;
    MostRecentEntry.LineNumber = LineNumber;   
}
