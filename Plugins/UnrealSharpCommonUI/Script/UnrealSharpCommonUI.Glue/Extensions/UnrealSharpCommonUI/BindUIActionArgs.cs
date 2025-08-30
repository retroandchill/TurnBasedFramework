using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnrealSharp.CommonInput;
using UnrealSharp.CommonUI;
using UnrealSharp.Core.Marshallers;
using UnrealSharp.CoreUObject;
using UnrealSharp.Engine;
using UnrealSharp.EnhancedInput;
using UnrealSharp.UMG;
using static UnrealSharp.UnrealSharpCommonUI.UBindUIActionArgsExtensions;

namespace UnrealSharp.UnrealSharpCommonUI;

[InlineArray(264)]
public struct FBindUIActionArgs
{
    private byte _padding;
}

internal enum GCHandleType : byte
{
    Null,
    StrongHandle,
    WeakHandle,
    PinnedHandle,
};


[StructLayout(LayoutKind.Sequential)]
internal struct CSManagedDelegate
{
    private readonly IntPtr _ptr;
    private readonly GCHandleType _type;
}

[StructLayout(LayoutKind.Sequential)]
internal struct FloatDelegateData
{
    private readonly CSManagedDelegate _managedDelegate;
    internal readonly float _value;
}

public record BindUIActionArgs
{
    public TWeakObjectPtr<UWidget> Widget { get; }
    internal readonly FBindUIActionArgs NativeData;

    public FName ActionName
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetActionName(ptr);
                }
            }
        }
    }

    public bool ActionHasHoldMappings
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return ActionHasHoldMappings(ptr);
                }
            }
        }
    }

    public FUIActionTag ActionTag
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetActionTag(ptr);
                }
            }
        }
    }

    public FDataTableRowHandle ActionTableRow
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetActionTableRow(ptr);
                }
            }
        }
    }

    public UInputAction? InputAction
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetInputAction(ptr);
                }
            }
        }
    }

    public ECommonInputMode InputMode
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetInputMode(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetInputMode(ptr, value);
                }
            }
        }
    }


    public EInputEvent KeyEvent
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetKeyEvent(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetKeyEvent(ptr, value);
                }
            }
        }
    }

    private IReadOnlySet<ECommonInputType>? _inputTypesExemptFromValidKeyCheck;
    public IReadOnlySet<ECommonInputType> InputTypesExemptFromValidKeyCheck
    {
        get
        {
            if (_inputTypesExemptFromValidKeyCheck is not null)
            {
                return _inputTypesExemptFromValidKeyCheck;           
            }
            
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    var result = GetInputTypesExemptFromValidKeyCheck(ptr);

                    if (result is IReadOnlySet<ECommonInputType> set)
                    {
                        _inputTypesExemptFromValidKeyCheck = set;
                    }
                    else
                    {
                        _inputTypesExemptFromValidKeyCheck = result.ToImmutableHashSet();
                    }
                }
            }
            
            return _inputTypesExemptFromValidKeyCheck;
        }
        init
        {
            _inputTypesExemptFromValidKeyCheck = value;

            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    if (value is ISet<ECommonInputType> set)
                    {
                        SetInputTypesExemptFromValidKeyCheck(ptr, set);
                    }
                    else
                    {
                        SetInputTypesExemptFromValidKeyCheck(ptr, value.ToHashSet());   
                    }
                }
            }
        }
    }

    public bool IsPersistent
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetIsPersistent(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetIsPersistent(ptr, value);
                }
            }
        }
    }

    public bool ConsumeInput
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetConsumeInput(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetConsumeInput(ptr, value);
                }
            }
        }
    }

    public bool DisplayInActionBar
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetDisplayInActionBar(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetDisplayInActionBar(ptr, value);
                }
            }
        }
    }

    public bool ForceHold
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetForceHold(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetForceHold(ptr, value);
                }
            }
        }
    }
    
    private FText? _overrideDisplayName;

    public FText OverrideDisplayName
    {
        get
        {
            if (_overrideDisplayName is not null)
            {
                return _overrideDisplayName;
            }

            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    _overrideDisplayName = GetOverrideDisplayName(ptr);
                }
            }

            return _overrideDisplayName;
        }
        init
        {
            _overrideDisplayName = value;
            
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetOverrideDisplayName(ptr, value);
                }
            }
        }
    }

    public int PriorityWithinCollection
    {
        get
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    return GetPriorityWithinCollection(ptr);
                }
            }
        }
        init
        {
            unsafe
            {
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    SetPriorityWithinCollection(ptr, value);
                }
            }
        }
    }

    public  Action? OnExecuteAction { get; }

    public Action<float>? OnHoldActionProgressed
    {
        get;
        init
        {
            field = value;
            if (value is null) return;
            
            unsafe
            {
                FGuid guid = Guid.NewGuid();

                void RespondGetAndProcessFloat()
                {
                    var callbackRef = GetBoundDelegateData(Widget.Object, guid);
                    var delegateData = BlittableMarshaller<FloatDelegateData>.FromNative(callbackRef._ref, 0);
                    field?.Invoke(delegateData._value);
                }

                var handle = GCHandle.Alloc((Action?)RespondGetAndProcessFloat);
                
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    BindOnHoldActionProgressed(ptr, Widget.Object, 
                        new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)), guid);
                }
            }
        }
    }


    public Action? OnHoldActionPressed
    {
        get;
        init
        {
            field = value;
            if (field is null) return;
            
            unsafe
            {
                var handle = GCHandle.Alloc(field);
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    BindOnHoldActionPressed(ptr, Widget.Object, 
                        new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)));
                }
            }
        }
    }

	
    public Action? OnHoldActionReleased
    {
        get;
        init
        {
            field = value;
            if (field is null) return;
            
            unsafe
            {
                var handle = GCHandle.Alloc(field);
                fixed (FBindUIActionArgs* ptr = &NativeData)
                {
                    BindOnHoldActionReleased(ptr, Widget.Object, 
                        new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)));
                }
            }
        }
    }

    static BindUIActionArgs()
    {
        unsafe
        {
            if (BindUIActionArgsSize > sizeof(FBindUIActionArgs))
            {
                throw new Exception("BindUIActionArgs size is too big");           
            }
        }
    }

    public BindUIActionArgs(UWidget widget, FUIActionTag actionTag, Action onExecuteAction) : this(widget, actionTag, true, onExecuteAction)
    {
    }
    
    public BindUIActionArgs(UWidget widget, FUIActionTag actionTag, bool shouldDisplayInActionBar, Action onExecuteAction)
    {
        Widget = widget;
        OnExecuteAction = onExecuteAction;
        unsafe
        {
            var handle = GCHandle.Alloc(onExecuteAction);

            fixed (FBindUIActionArgs* ptr = &NativeData)
            {
                ConstructFromActionTag(ptr, widget, actionTag,
                    shouldDisplayInActionBar, new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)));
            }
        }
    }

    public BindUIActionArgs(UWidget widget, FDataTableRowHandle rowHandle, Action onExecuteAction) : this(widget,
        rowHandle, true, onExecuteAction)
    {
        
    }

    public BindUIActionArgs(UWidget widget, FDataTableRowHandle rowHandle, bool shouldDisplayInActionBar,
                            Action onExecuteAction)
    {
        Widget = widget;
        OnExecuteAction = onExecuteAction;
        unsafe
        {
            var handle = GCHandle.Alloc(onExecuteAction);

            fixed (FBindUIActionArgs* ptr = &NativeData)
            {
                ConstructFromRowHandle(ptr, widget, rowHandle,
                    shouldDisplayInActionBar, new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)));
            }
        }
    }

    public BindUIActionArgs(UWidget widget, UInputAction inputAction, Action onExecuteAction) : this(widget,
        inputAction, true, onExecuteAction)
    {
        
    }
    
    public BindUIActionArgs(UWidget widget, UInputAction inputAction, bool shouldDisplayInActionBar,
                            Action onExecuteAction)
    {
        Widget = widget;
        OnExecuteAction = onExecuteAction;
        unsafe
        {
            var handle = GCHandle.Alloc(onExecuteAction);

            fixed (FBindUIActionArgs* ptr = &NativeData)
            {
                ConstructFromInputAction(ptr, widget, inputAction,
                    shouldDisplayInActionBar, new FManagedDelegateHandle(GCHandle.ToIntPtr(handle)));
            }
        }
    }

    public BindUIActionArgs(BindUIActionArgs other) 
    {
        Widget = other.Widget;
        OnExecuteAction = other.OnExecuteAction;
        unsafe
        {
            fixed (FBindUIActionArgs* fromPtr = &other.NativeData)
            fixed (FBindUIActionArgs* toPtr = &NativeData)
            {
                Copy(fromPtr, toPtr);
            }
        }
    }
    
    ~BindUIActionArgs()
    {
        unsafe
        {
            fixed (FBindUIActionArgs* ptr = &NativeData)
            {
                
                Release(ptr);
            }
        }
    }
}