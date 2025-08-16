#include "UnrealSharpTest.h"
#include "Misc/AutomationTest.h"
#include "Runner/CSharpTestLatentCommand.h"

IMPLEMENT_COMPLEX_AUTOMATION_TEST(
    FCSharpAutomationTest,
    "Managed.CSharp",
    EAutomationTestFlags::EditorContext | EAutomationTestFlags::EngineFilter)

void FCSharpAutomationTest::GetTests(TArray<FString>& OutBeautifiedNames, TArray<FString>& OutTestCommands) const
{
    for (const auto& [Name, IdMap] : FUnrealSharpTestModule::Get().GetTestIds())
    {
        for (const auto& TestCaseName : IdMap)
        {
            OutBeautifiedNames.Add(TestCaseName);
            OutTestCommands.Add(Name.ToString() + ":" + TestCaseName);
        }
    }
}

bool FCSharpAutomationTest::RunTest(const FString& Parameters)
{
    int32 SeparatorIndex;
    Parameters.FindChar(TEXT(':'), SeparatorIndex);
    const FName AssemblyName(FStringView(Parameters.GetCharArray().GetData(), SeparatorIndex));
    const FString TestCaseName = Parameters.Mid(SeparatorIndex + 1);

    const auto Command = MakeShared<FCSharpTestLatentCommand>(AssemblyName, TestCaseName);
    if (Command->Update())
    {
        return true;
    }

    FAutomationTestFramework::Get().EnqueueLatentCommand(Command);
    return true;
}
