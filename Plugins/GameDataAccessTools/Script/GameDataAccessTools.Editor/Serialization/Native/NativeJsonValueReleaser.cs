using ManagedGameDataAccessToolsEditor.Interop;

namespace ManagedGameDataAccessToolsEditor.Serialization.Native;

public ref struct NativeJsonValueReleaser(ref NativeJsonValue nativeJsonValue) : IDisposable
{
    private readonly ref NativeJsonValue _nativeJsonValue = ref nativeJsonValue;
    private bool _isDisposed;
    
    public void Dispose()
    {
        if (_isDisposed) return;
        
        FJsonValueExporter.CallDestroyJsonValue(ref _nativeJsonValue);
        _isDisposed = true;
    }
}