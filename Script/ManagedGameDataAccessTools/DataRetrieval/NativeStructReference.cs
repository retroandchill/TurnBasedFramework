using UnrealSharp.CoreUObject;

namespace ManagedGameDataAccessTools.DataRetrieval;

public interface INativeStructReference {
  static abstract UScriptStruct GetScriptStruct();
}

public interface INativeStructReference<T> : INativeStructReference where T : struct, INativeStructReference<T> {
  static abstract T Create(IntPtr nativeStruct);
}