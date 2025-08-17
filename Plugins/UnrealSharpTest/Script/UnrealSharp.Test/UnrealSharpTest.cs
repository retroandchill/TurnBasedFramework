using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using NUnit.VisualStudio.TestAdapter;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Interop;
using UnrealSharp.UnrealSharpTest;
using ManagedTestingExporter = UnrealSharp.Test.Interop.ManagedTestingExporter;

namespace UnrealSharp.Test;

[CustomLog]
public static partial class LogUnrealSharpTest;

public class FUnrealSharpTestModule : IModuleInterface
{
    
    public void StartupModule()
    {
        var actions = ManagedTestingActions.Create();
        ManagedTestingExporter.CallSetManagedActions(ref actions);
    }

    public void ShutdownModule()
    {
        var actions = default(ManagedTestingActions);
        ManagedTestingExporter.CallSetManagedActions(ref actions);
    }
}