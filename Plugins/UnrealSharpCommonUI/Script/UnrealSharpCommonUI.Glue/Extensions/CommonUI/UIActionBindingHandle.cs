using JetBrains.Annotations;
using UnrealSharp.Attributes;
using UnrealSharp.Core.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.Interop;
using UnrealSharp.UMG;
using UnrealSharp.UnrealSharpCommonUI;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace UnrealSharp.CommonUI;

[UStruct, GeneratedType("UIActionBindingHandle", "UnrealSharp.CommonUI.UIActionBindingHandle")]
[PublicAPI]
public struct FUIActionBindingHandle(IntPtr nativeStruct) : MarshalledStruct<FUIActionBindingHandle>
{
    private readonly int _registrationId = new FUIActionBindingHandleRef(nativeStruct).GetRegistrationId();

    public bool IsValid => this.IsValid();

    public void Unregister()
    {
        UUIActionBindingExtensions.Unregister(ref this);
    }

    public void ResetHold()
    {
        UUIActionBindingExtensions.ResetHold(ref this);
    }

    public FName ActionName => this.GetActionName();

    public FText DisplayName
    {
        get => this.GetDisplayName();
        set => UUIActionBindingExtensions.SetDisplayName(ref this, value);
    }

    public bool DisplayInActionBar
    {
        get => this.GetDisplayInActionBar();
        set => UUIActionBindingExtensions.SetDisplayInActionBar(ref this, value);
    }

    public UWidget? BoundWidget => this.GetBoundWidget();

    public ULocalPlayer? BoundLocalPlayer => this.GetBoundLocalPlayer();

    private static readonly IntPtr NativeClassPtr = UCoreUObjectExporter.CallGetNativeStructFromName(typeof(FUIActionBindingHandle).GetAssemblyName(), "UnrealSharp.CommonUI", "UIActionBindingHandle");
    public static IntPtr GetNativeClassPtr() => NativeClassPtr;
    public static readonly int NativeDataSize = UScriptStructExporter.CallGetNativeStructSize(NativeClassPtr);
    public static int GetNativeDataSize() => NativeDataSize;


    public static FUIActionBindingHandle FromNative(IntPtr buffer) => new(buffer);
    
    public void ToNative(IntPtr buffer)
    {
        new FUIActionBindingHandleRef(buffer).SetRegistrationId(_registrationId);
    }
}

[PublicAPI]
public static class FUIActionBindingHandleMarshaller
{
    public static FUIActionBindingHandle FromNative(IntPtr nativeBuffer, int arrayIndex)
    {
        return new FUIActionBindingHandle(nativeBuffer + (arrayIndex * GetNativeDataSize()));
    }
    
    public static void ToNative(IntPtr nativeBuffer, int arrayIndex, FUIActionBindingHandle obj)
    {
        obj.ToNative(nativeBuffer + (arrayIndex * GetNativeDataSize()));
    }
    
    public static int GetNativeDataSize()
    {
        return FUIActionBindingHandle.NativeDataSize;
    }
}