using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Retro.ReadOnlyParams.Annotations;

namespace UnrealSharp.Test.Model;

public sealed class RandomPlaceholder([ReadOnly] RandomAttribute randomAttribute, 
                                      [ReadOnly] IParameterInfo parameterInfo)
{
    public object? GetRandomValue()
    {
        return randomAttribute.GetData(parameterInfo).Cast<object>().FirstOrDefault();
    }
}