using System.Collections;
using System.Runtime.CompilerServices;
using ManagedGameDataAccessTools.Interop;
using Retro.ReadOnlyParams.Annotations;
using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.Interop;

namespace ManagedGameDataAccessTools.DataRetrieval;

public readonly ref struct DataTableProxyRow<T>(FName key, T value) where T : struct, INativeStructReference<T>, allows ref struct {
  public FName Key { get; } = key;
  public T Value { get; } = value;

  public void Deconstruct(out FName key, out T value) {
    key = this.Key;
    value = this.Value;
  }

}

public interface IDataTableProxy<T> : IEnumerable<DataTableProxyRow<T>>, IDisposable where T : struct, INativeStructReference<T>, allows ref struct {
  public IEnumerable<FName> Keys { get; }
  public IEnumerable<T> Values { get; }
  
}

public struct DataTableProxyUnsafe {
  internal IntPtr DataTable;
}

[InlineArray(64)]
public struct OpaqueValueStorage {
  private byte _padding;
}

public sealed class DataTableProxy<T> : IDataTableProxy<T>
    where T : struct, INativeStructReference<T>, allows ref struct {
  private DataTableProxyUnsafe _nativeProxy;
  private bool _disposed;

  public IEnumerable<FName> Keys => new SubEnumerator<FName>(new KeyEnumerator(this));
  public IEnumerable<T> Values => new SubEnumerator<T>(new ValueEnumerator(this));

  public int Count => DataTableProxyExporter.CallGetNumRows(ref _nativeProxy);

  public DataTableProxy(IntPtr dataTableRegistration) {
    if (T.GetNativeStruct() != DataTableProxyExporter.CallGetScriptStruct(dataTableRegistration)) {
      throw new InvalidOperationException("DataTable provided utilizes the wrong struct type.");
    }
    
    DataTableProxyExporter.CallInitDataTableProxy(ref _nativeProxy, dataTableRegistration);
  }
  
  ~DataTableProxy() {
    Dispose();
  }
  
  public static UScriptStruct ScriptStruct => T.GetScriptStruct();
  
  public IEnumerator<DataTableProxyRow<T>> GetEnumerator()
  {
      return new Enumerator(this);
  }
  
  IEnumerator IEnumerable.GetEnumerator()
  {
      return GetEnumerator();
  }
  
  public bool ContainsKey(FName key)
  {
      return DataTableProxyExporter.CallContainsKey(ref _nativeProxy, key);
  }
  public bool TryGetValue(FName key, out T value)
  {
      var existing = DataTableProxyExporter.CallGetDataFromRow(ref _nativeProxy, key);
      if (existing == IntPtr.Zero) {
        value = default;
        return false;
      }
      
      value = T.Create(existing);
      return true;
  }

  public T this[FName key] => TryGetValue(key, out var result) ? result : throw new KeyNotFoundException();

  public void Dispose() {
    if (_disposed) {
      return;
    }
    
    DataTableProxyExporter.CallDeinitDataTableProxy(ref _nativeProxy);
    _disposed = true;
    GC.SuppressFinalize(this);
  }

  private abstract class EnumeratorBase([ReadOnly] DataTableProxy<T> owner) : IEnumerator, IDisposable {
    private OpaqueValueStorage _nativeIterator;
    protected ref OpaqueValueStorage NativeIterator => ref _nativeIterator;
    protected bool Created { get; private set; }

    public bool MoveNext() {
      unsafe {
        if (Created) return DataTableProxyExporter.CallMoveNextNativeIterator(ref _nativeIterator);
        if (!DataTableProxyExporter.CallInitializeNativeIterator(ref _nativeIterator, sizeof(OpaqueValueStorage), ref owner._nativeProxy)) {
          throw new InvalidOperationException("Failed to create iterator.");
        }

        Created = true;
        return true;

      }
    }

    public void Reset() {
      Created = false;
    }
    
    object IEnumerator.Current => throw new InvalidOperationException("This enumerator cannot box");

    public void Dispose() {
      // Nothing to dispose
    }
  }
  
  private sealed class Enumerator(DataTableProxy<T> owner) : EnumeratorBase(owner), IEnumerator<DataTableProxyRow<T>> {
    public DataTableProxyRow<T> Current {
      get {
        if (Created && DataTableProxyExporter.CallGetNativeIteratorValue(ref NativeIterator, out var key, out var value)) {
          return new DataTableProxyRow<T>(key, T.Create(value));
        }
        
        throw new InvalidOperationException("Failed to get current value.");
      }
    }
  }
  
  private sealed class KeyEnumerator(DataTableProxy<T> owner) : EnumeratorBase(owner), IEnumerator<FName> {
    public FName Current {
      get {
        if (Created && DataTableProxyExporter.CallGetNativeIteratorValue(ref NativeIterator, out var key, out _)) {
          return key;
        }
        
        throw new InvalidOperationException("Failed to get current value.");
      }
    }
  }
  
  private sealed class ValueEnumerator(DataTableProxy<T> owner) : EnumeratorBase(owner), IEnumerator<T> {
    public T Current {
      get {
        if (Created && DataTableProxyExporter.CallGetNativeIteratorValue(ref NativeIterator, out _, out var value)) {
          return T.Create(value);
        }
        
        throw new InvalidOperationException("Failed to get current value.");
      }
    }
  }

  private class SubEnumerator<TSub>([ReadOnly] IEnumerator<TSub> enumerator) : IEnumerable<TSub> where TSub : struct, allows ref struct {

    public IEnumerator<TSub> GetEnumerator() {
      return enumerator;
    }
    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}