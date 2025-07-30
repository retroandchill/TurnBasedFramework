using System.Runtime.InteropServices;

namespace GameDataAccessTools.Editor.Serialization.Native;

[StructLayout(LayoutKind.Sequential)]
public struct NativeJsonValue
{
    public IntPtr Ptr;
    public IntPtr ReferenceController;
}