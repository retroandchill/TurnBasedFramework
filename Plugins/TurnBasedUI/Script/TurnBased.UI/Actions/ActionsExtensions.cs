using TurnBased.UI.Interop;
using UnrealSharp.CommonUI;

namespace TurnBased.UI.Actions;

public static class ActionsExtensions
{
    extension(UCommonUserWidget widget)
    {
        public IEnumerable<FUIActionBindingHandle> ActionBindings
        {
            get
            {
                ActionRegistrationExporter.CallGetActionBindings(widget.NativeObject, out var bindingsPtr, out var bindingsCount);
                for (var i = 0; i < bindingsCount; i++)
                {
                    yield return FUIActionBindingHandle.FromNative(bindingsPtr + i * FUIActionBindingHandle.NativeDataSize);
                }
            }
        }
        
        public FUIActionBindingHandle RegisterUIActionBinding(BindUIActionArgs args)
        {
            unsafe
            {
                Span<byte> handleBytes = stackalloc byte[FUIActionBindingHandle.NativeDataSize];
                fixed (byte* handlePtr = handleBytes)
                {
                    try
                    {
                        ActionRegistrationExporter.CallRegisterActionBinding(widget.NativeObject, ref args.Args,
                            (IntPtr)handlePtr);
                        return FUIActionBindingHandle.FromNative((IntPtr)handlePtr);
                    }
                    finally
                    {
                        ActionRegistrationExporter.CallDestructHandle((IntPtr) handlePtr);
                    }
                }
            }
        }

        public void AddActionBinding(FUIActionBindingHandle bindingHandle)
        {
            unsafe
            {
                Span<byte> handleBytes = stackalloc byte[FUIActionBindingHandle.NativeDataSize];
                fixed (byte* handlePtr = handleBytes)
                {
                    try
                    {
                        bindingHandle.ToNative((IntPtr)handlePtr);
                        ActionRegistrationExporter.CallAddActionBinding(widget.NativeObject, (IntPtr)handlePtr);
                    }
                    finally
                    {
                        ActionRegistrationExporter.CallDestructHandle((IntPtr) handlePtr);
                    }
                }
            }
        }

        public void RemoveActionBinding(FUIActionBindingHandle bindingHandle)
        {
            unsafe
            {
                Span<byte> handleBytes = stackalloc byte[FUIActionBindingHandle.NativeDataSize];
                fixed (byte* handlePtr = handleBytes)
                {
                    try
                    {
                        bindingHandle.ToNative((IntPtr)handlePtr);
                        ActionRegistrationExporter.CallAddActionBinding(widget.NativeObject, (IntPtr)handlePtr);
                    }
                    finally
                    {
                        ActionRegistrationExporter.CallDestructHandle((IntPtr) handlePtr);
                    }
                }
            }
        }
    }
}