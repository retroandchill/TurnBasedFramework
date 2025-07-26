using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;

namespace ManagedNativeReflectionAccessors.Utils;

/// <summary>
/// A struct used to manage the lifecycle of an unmanaged array of characters.
/// </summary>
/// <remarks>
/// StringDisposer facilitates the allocation and deallocation of unmanaged resources
/// represented by the UnmanagedArray, ensuring proper cleanup through the implementation
/// of IDisposable. This struct provides a safe mechanism to handle unmanaged memory
/// tied to string operations.
/// </remarks>
/// <threadsafety>
/// This struct is not thread-safe and its members cannot be used concurrently across multiple threads.
/// Ensure that instances are accessed by a single thread at any given time to avoid data corruption or unexpected behavior.
/// </threadsafety>
/// <responsibilities>
/// Responsible for initializing, managing, and properly deconstructing its associated unmanaged memory array.
/// Used for marshalling unmanaged string data in interop scenarios.
/// </responsibilities>
/// <usage>
/// Use this struct when interacting with unmanaged resources that require manual memory management.
/// Ensure that the Dispose method is called to release resources when the instance is no longer needed.
/// </usage>
public struct StringDisposer() : IDisposable
{
    /// <summary>
    /// Represents a raw unmanaged array used for native memory interactions.
    /// </summary>
    public readonly UnmanagedArray NativeArray = new();

    /// <inheritdoc />
    public void Dispose()
    {
        unsafe
        {
            fixed (void* p = &NativeArray)
            {
                StringMarshaller.DestructInstance((IntPtr) p, 0);
            }
        }
    }
}