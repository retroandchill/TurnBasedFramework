using System.Collections;
using System.Reflection;
using NUnit.Framework.Interfaces;

namespace UnrealSharp.Test.Model;

public interface IDataPlaceholder
{
    int Index { get; }
    
    ParameterInfo ParameterInfo { get; }

    object?[] GetData();
}