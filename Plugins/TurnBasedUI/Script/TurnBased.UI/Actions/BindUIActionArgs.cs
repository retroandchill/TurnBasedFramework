using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TurnBased.UI.Interop;
using UnrealSharp;
using UnrealSharp.CommonInput;
using UnrealSharp.CommonUI;
using UnrealSharp.Core;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;

namespace TurnBased.UI.Actions;

[StructLayout(LayoutKind.Sequential)]
public struct FDataTableRowHandleNative
{
    internal IntPtr DataTable;
    internal FName RowName;
}

[InlineArray(80)]
public struct CommonInputTypeSetData
{
    private byte _dummy;
}

[InlineArray(24)]
public struct NativeDelegate
{
    private byte _dummy;
}

[StructLayout(LayoutKind.Sequential)]
public struct FBindUIActionArgs {
    internal FUIActionTag ActionTag;
    internal FDataTableRowHandleNative ActionTableRow;
    internal TWeakObjectPtr<UInputAction> InputAction;
    internal ECommonInputMode InputMode;
    internal EInputEvent KeyEvent;
    internal CommonInputTypeSetData InputTypesExemptFromValidKeyCheck;
    internal NativeBool IsPersistent;
    internal NativeBool ConsumeInput;
    internal NativeBool DisplayInActionBar;
    internal NativeBool ForceHold;
    internal FTextData OverrideDisplayName;
    internal int PriorityWithinCollection;
    internal NativeDelegate OnExecuteAction;
    internal NativeDelegate OnHoldActionProgressed;
    internal NativeDelegate OnHoldActionPressed;
    internal NativeDelegate OnHoldActionReleased;
}

public sealed class BindUIActionArgs : IDisposable
{
    internal FBindUIActionArgs Args;

    public FUIActionTag ActionTag
    {
        get => Args.ActionTag;
        init => Args.ActionTag = value;
    }

    public FDataTableRowHandle ActionTableRow
    {
        get
        {
            unsafe
            {
                fixed (FDataTableRowHandleNative* ptr = &Args.ActionTableRow)
                {
                    return FDataTableRowHandle.FromNative((IntPtr)ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FDataTableRowHandleNative* ptr = &Args.ActionTableRow)
                {
                    value.ToNative((IntPtr)ptr);
                }
            }
        }
    }

    public TWeakObjectPtr<UInputAction> InputAction
    {
        get => Args.InputAction;
        init => Args.InputAction = value;
    }

    public ECommonInputMode InputMode
    {
        get => Args.InputMode;
        init => Args.InputMode = value;
    }

    public EInputEvent KeyEvent
    {
        get => Args.KeyEvent;
        init => Args.KeyEvent = value;
    }

    private static readonly IntPtr ExemptInputTypesProperty = BindUIActionArgsExporter.CallGetExemptInputTypesProperty();
    private SetReadOnlyMarshaller<ECommonInputType>? _exemptInputTypesMarshaller;

    public IReadOnlySet<ECommonInputType> ExemptInputTypes
    {
        get
        {
            _exemptInputTypesMarshaller ??= new SetReadOnlyMarshaller<ECommonInputType>(ExemptInputTypesProperty, 
                EnumMarshaller<ECommonInputType>.ToNative, EnumMarshaller<ECommonInputType>.FromNative);
            
            unsafe
            {
                fixed (CommonInputTypeSetData* ptr = &Args.InputTypesExemptFromValidKeyCheck)
                {
                    return _exemptInputTypesMarshaller.FromNative((IntPtr)ptr, 0);
                }
            }
        }
        init
        {
            _exemptInputTypesMarshaller ??= new SetReadOnlyMarshaller<ECommonInputType>(ExemptInputTypesProperty, 
                EnumMarshaller<ECommonInputType>.ToNative, EnumMarshaller<ECommonInputType>.FromNative);

            unsafe
            {
                fixed (CommonInputTypeSetData* ptr = &Args.InputTypesExemptFromValidKeyCheck)
                {
                    _exemptInputTypesMarshaller.ToNative((IntPtr)ptr, value);
                }
            }
        }
    }

    public bool IsPersistent
    {
        get => Args.IsPersistent.ToManagedBool();
        init => Args.IsPersistent = value.ToNativeBool();
    }

    public bool ConsumeInput
    {
        get => Args.ConsumeInput.ToManagedBool();
        init => Args.ConsumeInput = value.ToNativeBool();
    }

    public bool DisplayInActionBar
    {
        get => Args.DisplayInActionBar.ToManagedBool();
        init => Args.DisplayInActionBar = value.ToNativeBool();
    }

    public bool ForceHold
    {
        get => Args.ForceHold.ToManagedBool();
        init => Args.ForceHold = value.ToNativeBool();
    }

    public FText OverrideDisplayName
    {
        get
        {
            unsafe
            {
                fixed (FTextData* ptr = &Args.OverrideDisplayName)
                {
                    return TextMarshaller.FromNative((IntPtr)ptr, 0);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FTextData* ptr = &Args.OverrideDisplayName)
                {
                    TextMarshaller.ToNative((IntPtr)ptr, 0, value);   
                }
            }
        }
    }

    public int PriorityWithinCollection
    {
        get => Args.PriorityWithinCollection;
        init => Args.PriorityWithinCollection = value;   
    }

    public Action? OnExecuteAction
    {
        get;
        init
        {
            field = value;
            if (value is null) return;

            var handle = GCHandle.Alloc(value);
            BindUIActionArgsExporter.CallBindNoArgsDelegate(ref Args.OnExecuteAction, GCHandle.ToIntPtr(handle));
        }
    }

    public Action<float>? OnHoldActionProgressed
    {
        get;
        init
        {
            field = value;
            if (value is null) return;
            
            var handle = GCHandle.Alloc(value);
            BindUIActionArgsExporter.CallBindFloatDelegate(ref Args.OnHoldActionProgressed, GCHandle.ToIntPtr(handle));
        }
    }

    public Action? OnHoldActionPressed
    {
        get;
        init
        {
            field = value;
            if (value is null) return;

            var handle = GCHandle.Alloc(value);
            BindUIActionArgsExporter.CallBindNoArgsDelegate(ref Args.OnHoldActionPressed, GCHandle.ToIntPtr(handle));
        }
    }

    public Action? OnHoldActionReleased
    {
        get;
        init
        {
            field = value;
            if (value is null) return;

            var handle = GCHandle.Alloc(value);
            BindUIActionArgsExporter.CallBindNoArgsDelegate(ref Args.OnHoldActionReleased, GCHandle.ToIntPtr(handle));
        }
    }

    public FName ActionName => BindUIActionArgsExporter.CallGetActionName(ref Args);

    public bool ActionHasHoldMappings => BindUIActionArgsExporter.CallActionHasHoldMappings(ref Args).ToManagedBool();
    
    public BindUIActionArgs(FUIActionTag actionTag, Action onExecuteAction)
    {
        BindUIActionArgsExporter.CallConstructFromActionTag(ref Args, actionTag, GCHandle.ToIntPtr(GCHandle.Alloc(onExecuteAction)));
    }
    
    public BindUIActionArgs(FUIActionTag actionTag, bool shouldDisplayInActionBar, Action onExecuteAction) : this(actionTag, onExecuteAction)
    {
        DisplayInActionBar = shouldDisplayInActionBar;   
    }
    
    public BindUIActionArgs(FDataTableRowHandle rowHandle, Action onExecuteAction)
    {
        BindUIActionArgsExporter.CallConstructFromRowHandle(ref Args, rowHandle.DataTable.NativeObject, rowHandle.RowName, GCHandle.ToIntPtr(GCHandle.Alloc(onExecuteAction)));
    }
    
    public BindUIActionArgs(FDataTableRowHandle rowHandle, bool shouldDisplayInActionBar, Action onExecuteAction) : this(rowHandle, onExecuteAction)
    {
        DisplayInActionBar = shouldDisplayInActionBar;   
    }

    public BindUIActionArgs(UInputAction inputAction, Action onExecuteAction)
    {
        BindUIActionArgsExporter.CallConstructFromInputAction(ref Args, inputAction.NativeObject, GCHandle.ToIntPtr(GCHandle.Alloc(onExecuteAction)));
    }

    public BindUIActionArgs(UInputAction inputAction, bool shouldDisplayInActionBar, Action onExecuteAction) : this(inputAction, onExecuteAction)
    {
        DisplayInActionBar = shouldDisplayInActionBar;  
    }

    ~BindUIActionArgs()
    {
        Dispose();   
    }

    public void Dispose()
    {
        BindUIActionArgsExporter.CallDestruct(ref Args);
        GC.SuppressFinalize(this);  
    }
}