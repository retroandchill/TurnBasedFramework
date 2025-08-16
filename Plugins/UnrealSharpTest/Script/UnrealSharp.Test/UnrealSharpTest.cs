using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;
using UnrealSharp.Test.Interop;
using UnrealSharp.UnrealSharpTest;
using ManagedTestingExporter = UnrealSharp.Test.Interop.ManagedTestingExporter;

namespace UnrealSharp.Test;

[CustomLog]
public static partial class LogUnrealSharpTest;

public class FUnrealSharpTestModule : IModuleInterface
{
    private static FUnrealSharpTestModule? _instance;

    public static FUnrealSharpTestModule Instance
    {
        get
        {
            if (_instance is null)
            {
                throw new InvalidOperationException("Module not initialized");
            }
            
            return _instance;
        }
    }
    
    public void StartupModule()
    {
        var actions = ManagedTestingActions.Create();
        ManagedTestingExporter.CallSetManagedActions(ref actions);
        
        _instance = this;
    }

    public void ShutdownModule()
    {
        _instance = null;
    }
}