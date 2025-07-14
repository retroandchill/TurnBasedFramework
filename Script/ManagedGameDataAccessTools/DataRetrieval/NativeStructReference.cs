using UnrealSharp.CoreUObject;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface INativeStructReference {
  static abstract UScriptStruct GetScriptStruct();
  static abstract IntPtr GetNativeStruct();
}

public interface INativeStructReference<T> : INativeStructReference where T : struct, INativeStructReference<T>, allows ref struct {
  static abstract T Create(IntPtr nativeStruct);
}