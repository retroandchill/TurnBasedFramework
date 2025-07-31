using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;

namespace GameDataAccessTools.Core.Serialization.Native;

public ref struct StringDataReleaser(ref UnmanagedArray stringData) : IDisposable
{
    private readonly ref UnmanagedArray _stringData = ref stringData;
    private bool _isDisposed;
    
    public void Dispose()
    {
        if (_isDisposed) return;

        unsafe
        {
            fixed (UnmanagedArray* ptr = &_stringData)
            {
                var intPtr = (IntPtr)ptr;
                StringMarshaller.DestructInstance(intPtr, 0);
            }
        }
        
        _isDisposed = true;
    }
}