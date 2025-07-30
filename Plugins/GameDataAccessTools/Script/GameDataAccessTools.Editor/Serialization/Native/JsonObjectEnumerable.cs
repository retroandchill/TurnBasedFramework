using System.Collections;
using System.Runtime.CompilerServices;
using GameDataAccessTools.Editor.Interop;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;

namespace GameDataAccessTools.Editor.Serialization.Native;

public readonly ref struct JsonKeyValuePair( string key, ref NativeJsonValue value)
{
    public string Key { get; } = key;
    
    private readonly ref NativeJsonValue _value = ref value;
    public ref NativeJsonValue Value => ref _value;
}

[InlineArray(64)]
public struct MapIterator
{
    private byte _dummy;
}

public ref struct JsonObjectEnumerator(ref NativeJsonValue nativeJsonValue)
{
    private readonly ref NativeJsonValue _nativeJsonValue = ref nativeJsonValue;
    private MapIterator _nativeIterator;
    private bool _isInitialized;
    public JsonKeyValuePair Current { get; private set; }

    public bool MoveNext()
    {
        bool result;
        if (_isInitialized)
        {
            result = FJsonObjectExporter.CallAdvanceJsonIterator(ref _nativeIterator).ToManagedBool();
        }
        else
        {
            FJsonObjectExporter.CallCreateJsonIterator(ref _nativeJsonValue, ref _nativeIterator);
            _isInitialized = true;
            result = FJsonObjectExporter.CallIsValidJsonIterator(ref _nativeIterator).ToManagedBool();
        }

        if (result)
        {
            unsafe
            {
                var stringPtr = IntPtr.Zero;
                NativeJsonValue* valuePtr = null;
                FJsonObjectExporter.CallGetJsonIteratorValues(ref _nativeIterator, ref stringPtr, ref valuePtr);
                Current = new JsonKeyValuePair(StringMarshaller.FromNative(stringPtr, 0), ref *valuePtr);
            }
        }
        
        return result;
    }
}

public readonly ref struct JsonObjectEnumerable(ref NativeJsonValue nativeJsonValue)
{
    private readonly ref NativeJsonValue _nativeJsonValue = ref nativeJsonValue;

    public JsonObjectEnumerator GetEnumerator()
    {
        return new JsonObjectEnumerator(ref _nativeJsonValue);
    }
}