using System.Collections;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp.Test.Attributes;

namespace UnrealSharp.Test.Model;

public sealed class RandomPlaceholder([ReadOnly] DynamicRandomAttribute randomAttribute, 
                                      ParameterInfo parameterInfo,
                                      int index) : IDataPlaceholder
{
    public int Index { get; } = index;
    public ParameterInfo ParameterInfo { get; } = parameterInfo;

    public object?[] GetData()
    {
        var random = new Randomizer();
        var dataSource = randomAttribute.GetDataSource(ParameterInfo);
        return dataSource.CollectValues(random, randomAttribute.Distinct);
    }

    public override string ToString()
    {
        return $"?{Index + 1}";
    }
}