using System.Runtime.InteropServices;

namespace GameDataAccessTools.Core.Serialization.Native;

[StructLayout(LayoutKind.Sequential)]
public struct NativeJsonValue
{
    public IntPtr Ptr;
    public IntPtr ReferenceController;
}