// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/AssertionExporter.h"

#include "Runner/CSharpAutomationTest.h"

void UAssertionExporter::RecordAssertion(const TWeakPtr<FCSharpAutomationTest>& Test, const TCHAR* Message,
                                         const EAutomationEventType Severity, const TCHAR* Filename, int32 LineNumber)
{
    const auto StrongTest = Test.Pin();
    check(StrongTest != nullptr);

    switch (Severity)
    {
    case EAutomationEventType::Info:
        StrongTest->LogInfo(Message, Filename, LineNumber);
        break;
    case EAutomationEventType::Warning:
        StrongTest->LogWarning(Message, Filename, LineNumber);
        break;
    case EAutomationEventType::Error:
        StrongTest->LogError(Message, Filename, LineNumber);
        break;
    default:
        UE_LOG(LogAutomationTestFramework, Fatal, TEXT("Unknown severity %d"), Severity);
    }
}
