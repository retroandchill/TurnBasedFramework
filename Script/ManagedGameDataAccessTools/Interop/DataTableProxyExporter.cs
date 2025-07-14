using ManagedGameDataAccessTools.DataRetrieval;
using UnrealSharp;
using UnrealSharp.Binds;

namespace ManagedGameDataAccessTools.Interop;

[NativeCallbacks]
public static unsafe partial class DataTableProxyExporter { 
  public static delegate* unmanaged<ref DataTableProxyUnsafe, IntPtr, void> InitDataTableProxy;
  public static delegate* unmanaged<ref DataTableProxyUnsafe, void> DeinitDataTableProxy;
  public static delegate* unmanaged<IntPtr, IntPtr> GetScriptStruct;
  public static delegate* unmanaged<ref DataTableProxyUnsafe, FName, bool> ContainsKey;
  public static delegate* unmanaged<ref DataTableProxyUnsafe, int> GetNumRows;
  public static delegate* unmanaged<ref DataTableProxyUnsafe, FName, IntPtr> GetDataFromRow;
  
  public static delegate* unmanaged<ref OpaqueValueStorage, int, ref DataTableProxyUnsafe, bool> InitializeNativeIterator;
  public static delegate* unmanaged<ref OpaqueValueStorage, bool> MoveNextNativeIterator;
  public static delegate* unmanaged<ref OpaqueValueStorage, out FName, out IntPtr, bool> GetNativeIteratorValue;
}