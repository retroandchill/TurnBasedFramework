// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Model/ManagedTestCase.h"

/**
 * 
 */
class UNREALSHARPTEST_API FCSharpAutomationTest final : public FAutomationTestBase, public TSharedFromThis<FCSharpAutomationTest>
{
public:
    explicit FCSharpAutomationTest(FManagedTestCaseHandle TestCase)
        : FAutomationTestBase(TestCase.TestCase.FullyQualifiedName, false), ManagedTestCase(MoveTemp(TestCase))
    {
    }

    FString GetTestSourceFileName() const override;
    int32 GetTestSourceFileLine() const override;
    EAutomationTestFlags GetTestFlags() const override;
    FString GetBeautifiedTestName() const override;
    uint32 GetRequiredDeviceNum() const override;

    void LogInfo(FStringView Message, FStringView SourceFile, int32 LineNumber);
    void LogWarning(FStringView Message, FStringView SourceFile, int32 LineNumber);
    void LogError(FStringView Message, FStringView SourceFile, int32 LineNumber);

protected:
    void GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const override;
    bool RunTest(const FString& Parameters) override;

private:
    void OverrideTargetFile(FStringView SourceFile, int32 LineNumber);
    
    FManagedTestCaseHandle ManagedTestCase;
};
