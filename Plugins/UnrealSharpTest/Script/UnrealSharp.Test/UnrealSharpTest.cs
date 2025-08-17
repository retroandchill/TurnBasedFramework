using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NUnit.Engine.Services;
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

    private readonly List<Func<ITestDiscoverer>> _discoverers = [];
    public IReadOnlyList<Func<ITestDiscoverer>> Discoverers => _discoverers;
    
    public void StartupModule()
    {
        var actions = ManagedTestingActions.Create();
        ManagedTestingExporter.CallSetManagedActions(ref actions);

        AddDiscoverer<NUnit3TestDiscoverer>();
        
        _instance = this;
    }

    public void ShutdownModule()
    {
        _discoverers.Clear();
        _instance = null;
    }

    public FUnrealSharpTestModule AddDiscoverer(Func<ITestDiscoverer> discovererFactory)
    {
        _discoverers.Add(discovererFactory);
        return this;
    }

    public FUnrealSharpTestModule AddDiscoverer<TDiscoverer>() where TDiscoverer : ITestDiscoverer, new()
    {
        return AddDiscoverer(() => new TDiscoverer());
    }
}