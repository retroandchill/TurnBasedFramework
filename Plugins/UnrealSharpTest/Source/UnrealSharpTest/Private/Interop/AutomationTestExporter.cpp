// Fill out your copyright notice in the Description page of Project Settings.


#include "Interop/AutomationTestExporter.h"

#include "UnrealSharpTest.h"
#include "Runner/CSharpAutomationTest.h"
#include "Runner/CSharpTestLatentCommand.h"

class FFunctionalTestBase;

void UAutomationTestExporter::EnqueueLatentCommand(FCSharpAutomationTest* Test, const FGCHandleIntPtr TaskPtr)
{
    check(Test != nullptr);
    
    ADD_LATENT_AUTOMATION_COMMAND(FCSharpTestLatentCommand(Test->AsShared(), TaskPtr));
}

void UAutomationTestExporter::LogEvent(const TCHAR* Message, const EAutomationEventType EventType)
{
    if (const auto CurrentTest = FAutomationTestFramework::Get().GetCurrentTest(); CurrentTest != nullptr)
    {
            CurrentTest->AddEvent(FAutomationEvent(EventType, Message));
    }
    else
    {
        UE_LOG(LogUnrealSharpTestNative, Warning, TEXT("Tried to log a message for a C# test outside of the testing framework."));

        switch (EventType)
        {
        case EAutomationEventType::Info:
            UE_LOG(LogUnrealSharpTestNative, Display, TEXT("%s"), Message); 
            break;
        case EAutomationEventType::Warning:
            UE_LOG(LogUnrealSharpTestNative, Warning, TEXT("%s"), Message); 
            break;
        case EAutomationEventType::Error:
            UE_LOG(LogUnrealSharpTestNative, Error, TEXT("%s"), Message); 
            break;
        }
    }
}
