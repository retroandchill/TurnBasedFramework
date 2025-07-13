using UnrealSharp;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.Interop;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface IDataTableProxy<T> : IDisposable where T : struct, INativeStructReference<T> {
  
}

internal struct DataTableProxyUnsafe {
  internal IntPtr VTable;
  internal IntPtr DataTable;
}

public sealed class DataTableProxy<T> : IDataTableProxy<T>
    where T : struct, INativeStructReference<T> {
  private DataTableProxyUnsafe _nativeProxy;

  public DataTableProxy(IntPtr dataTableRegistration) {
    
  }
  
  public static UScriptStruct ScriptStruct => T.GetScriptStruct();

  public T GetRow(FName id) {
    return default;
  }

  public void Dispose() {
    
  }
}