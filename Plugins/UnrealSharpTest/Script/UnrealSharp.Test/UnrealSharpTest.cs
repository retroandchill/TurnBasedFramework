using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;
using UnrealSharp.Test.Interop;
using UnrealSharp.Test.Runner;
using UnrealSharp.Test.Runner.NUnit;
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
    
    private readonly Dictionary<FName, IUnrealSharpTestRunner> _runners = new();
    private readonly List<IUnrealSharpTestRunnerFactory> _runnerFactories = [];
    
    public void StartupModule()
    {
        var actions = ManagedTestingActions.Create();
        ManagedTestingExporter.CallSetManagedActions(ref actions);
        
        RegisterRunnerFactory<NUnitTestRunnerFactory>();
        
        _instance = this;
    }

    public void ShutdownModule()
    {
        _instance = null;
    }

    internal IUnrealSharpTestRunner? RegisterRunner(FName name, Assembly assembly)
    {
        if (_runners.ContainsKey(name))
        {
            LogUnrealSharpTest.LogError($"Tests for {name} already registered");
            return null;
        }

        var factory = _runnerFactories.FirstOrDefault(x => x.IsTestAssembly(assembly));
        if (factory is null) return null;
        
        try
        {
            var runner = factory.CreateRunner(name, assembly);
            _runners.Add(name, runner);
            
            var loadContext = AssemblyLoadContext.GetLoadContext(assembly);
            if (loadContext is not null)
            {
                loadContext.Unloading += _ => UnregisterRunner(name);
            }
            
            return runner;
        }
        catch (Exception e)
        {
            LogUnrealSharpTest.LogError($"Failed to register tests for {name}: {e}");
            return null;
        }
    }

    internal bool TryGetRunner(FName name, [NotNullWhen(true)] out IUnrealSharpTestRunner? runner)
    {
        return _runners.TryGetValue(name, out runner);
    }

    internal void UnregisterRunner(FName name)
    {
        _runners.Remove(name);
    }
    
    public FUnrealSharpTestModule RegisterRunnerFactory(IUnrealSharpTestRunnerFactory factory)
    {
        _runnerFactories.Add(factory);
        return this;
    }

    public FUnrealSharpTestModule RegisterRunnerFactory<T>() where T : IUnrealSharpTestRunnerFactory, new()
    {
        return RegisterRunnerFactory(new T());
    }

    public FUnrealSharpTestModule UnregisterRunnerFactory<T>() where T : IUnrealSharpTestRunnerFactory
    {
        _runnerFactories.RemoveAll(x => x is T);
        return this;
    }
}