using NUnit.Framework;
using Riok.Mapperly.Abstractions;
using UnrealSharp.Test.Model;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class TestCaseMapper
{
    public static partial FManagedTestCase ToManagedTestCase(this UnrealTestMethod testMethod);

    [MapperIgnoreTarget(nameof(TestCaseAttribute.Arguments))]
    public static partial TestCaseData ToTestCaseData(this TestCaseAttribute testCaseAttribute);

    [ObjectFactory]
    private static TestCaseData CreateTestCaseData(TestCaseAttribute testCaseAttribute)
    {
        return new TestCaseData(testCaseAttribute.Arguments);
    }
}