#include "Misc/AutomationTest.h"
#include "Runner/CSharpAutomationTestBase.h"

IMPLEMENT_CUSTOM_COMPLEX_AUTOMATION_TEST(
    FCSharpAutomationTest,
    FCSharpAutomationTestBase,
    "Managed.CSharp",
    EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)

void FCSharpAutomationTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
    CollectCSharpTests(OutBeautifiedNames, OutTestCommands);
}

bool FCSharpAutomationTest::RunTest(const FString& Parameters)
{
    bool Result = false;

    

    return Result;
}
