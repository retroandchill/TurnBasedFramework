using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Riok.Mapperly.Abstractions;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class TestCaseMapper
{
    public static partial FManagedTestCase ToManagedTestCase(this TestCase testCase);

    public static TestCase ToTestCase(this ref readonly FManagedTestCase testCase)
    {
        return new TestCase(testCase.FullyQualifiedName, new Uri(testCase.ExecutorUri), testCase.Source);
    }
}