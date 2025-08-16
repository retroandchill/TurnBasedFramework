// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "ManagedTestCase.h"

/**
 * 
 */
class UNREALSHARPTEST_API FCSharpAutomationTest : public FAutomationTestBase
{
public:
    explicit FCSharpAutomationTest(FManagedTestCase TestCase)
        : FAutomationTestBase(TestCase.FullyQualifiedName, false), ManagedTestCase(MoveTemp(TestCase))
    {
    }

    FString GetTestSourceFileName() const override
    {
        return ManagedTestCase.Source;
    }

    int32 GetTestSourceFileLine() const override
    {
        return ManagedTestCase.LineNumber;
    }

    EAutomationTestFlags GetTestFlags() const override;
    FString GetBeautifiedTestName() const override;
    uint32 GetRequiredDeviceNum() const override;

protected:
    void GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const override;
    bool RunTest(const FString& Parameters) override;

private:
    FManagedTestCase ManagedTestCase;
};
