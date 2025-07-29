using System.Runtime.InteropServices;

namespace ManagedGameDataAccessToolsEditor.Serialization.Native;

[StructLayout(LayoutKind.Sequential)]
public struct NativeJsonValue
{
    public IntPtr Ptr;
    public IntPtr ReferenceController;
}