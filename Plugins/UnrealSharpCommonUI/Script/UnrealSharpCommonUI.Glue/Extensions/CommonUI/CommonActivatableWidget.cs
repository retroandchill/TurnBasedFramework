// ReSharper disable once CheckNamespace

using UnrealSharp.UnrealSharpCommonUI;

namespace UnrealSharp.CommonUI;

public partial class UCommonActivatableWidget
{
    public IList<FUIActionBindingHandle> ActionBindings => UActionBindingExtensions.GetActionBindings(this);

    public FUIActionBindingHandle RegisterUIActionBinding(BindUIActionArgs args)
    {
        unsafe
        {
            fixed (FBindUIActionArgs* ptr = &args.NativeData)
            {
                return UActionBindingExtensions.RegisterActionBinding(this, ptr);
            }
        }
    }

    public void AddActionBinding(FUIActionBindingHandle handle)
    {
        UActionBindingExtensions.AddActionBinding(this, handle);
    }

    public void RemoveActionBinding(FUIActionBindingHandle handle)
    {
        UActionBindingExtensions.RemoveActionBinding(this, handle);
    }
}