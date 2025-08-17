using Riok.Mapperly.Abstractions;
using UnrealSharp.Test.Model;
using UnrealSharp.UnrealSharpTest;

namespace UnrealSharp.Test.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, PreferParameterlessConstructors = false)]
public static partial class TestCaseMapper
{
    public static partial FManagedTestCase ToManagedTestCase(this UnrealTestCase testCase);
}