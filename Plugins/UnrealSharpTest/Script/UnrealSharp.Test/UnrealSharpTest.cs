using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
using JetBrains.Annotations;
using NUnit.VisualStudio.TestAdapter;
using UnrealSharp.Engine.Core.Modules;
using UnrealSharp.Log;
using UnrealSharp.Test.Discovery;
using UnrealSharp.Test.Interop;
using UnrealSharp.UnrealSharpTest;
using ManagedTestingExporter = UnrealSharp.Test.Interop.ManagedTestingExporter;

namespace UnrealSharp.Test;

/// <summary>
/// Provides logging functionalities specific to UnrealSharp testing operations.
/// This class is decorated with a <c>CustomLog</c> attribute to enable custom
/// logging behavior. It is utilized across various test-related components
/// for error, debugging, and general logging purposes.
/// </summary>
[CustomLog]
public static partial class LogUnrealSharpTest;

/// <summary>
/// Represents a test module interface specifically for UnrealSharp.
/// This class manages the setup and teardown mechanisms required for
/// integrating managed testing actions within UnrealSharp's testing framework.
/// It registers and deregisters delegated testing operations from the
/// unmanaged system through the <c>ManagedTestingExporter</c>.
/// </summary>
[UsedImplicitly]
public class FUnrealSharpTestModule : IModuleInterface
{
    /// <inheritdoc />
    public void StartupModule()
    {
        var actions = ManagedTestingActions.Create();
        ManagedTestingExporter.CallSetManagedActions(ref actions);
    }

    /// <inheritdoc />
    public void ShutdownModule()
    {
        var actions = default(ManagedTestingActions);
        ManagedTestingExporter.CallSetManagedActions(ref actions);
    }
}
