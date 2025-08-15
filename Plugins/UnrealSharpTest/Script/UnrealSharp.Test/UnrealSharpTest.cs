using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Interop;
using ManagedTestingExporter = UnrealSharp.Test.Interop.ManagedTestingExporter;

namespace UnrealSharp.Test;

[CustomLog]
public static partial class LogUnrealSharpTest;

public class FUnrealSharpTest : IModuleInterface
{
    private static FUnrealSharpTest? _instance;

    public static FUnrealSharpTest Instance
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
    
    private UnifiedTestDiscoverer? _discoverer;

    public UnifiedTestDiscoverer Discoverer
    {
        get
        {
            _discoverer ??= new UnifiedTestDiscoverer();
            return _discoverer;
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