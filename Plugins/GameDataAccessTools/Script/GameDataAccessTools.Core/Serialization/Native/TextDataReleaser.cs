using UnrealSharp;

namespace GameDataAccessTools.Core.Serialization.Native;

public ref struct TextDataReleaser(ref FTextData textData) : IDisposable
{
    private readonly ref FTextData _textData = ref textData;
    private bool _isDisposed;
    
    public void Dispose()
    {
        if (_isDisposed) return;
        
        _textData.ObjectPointer.Release();
        _isDisposed = true;
    }
}