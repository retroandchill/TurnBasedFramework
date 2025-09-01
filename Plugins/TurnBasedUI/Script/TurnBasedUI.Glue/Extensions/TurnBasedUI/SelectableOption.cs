using System.Diagnostics.CodeAnalysis;
using LanguageExt;
using UnrealSharp.EnhancedInput;

namespace UnrealSharp.TurnBasedUI;

public partial record struct FSelectableOption
{
    [SetsRequiredMembers]
    public FSelectableOption(FName id, FText text, UInputAction? inputAction = null)
    {
        Id = id;
        Text = text;
        InputAction = inputAction;
    }
    
    [SetsRequiredMembers]
    public FSelectableOption(FName id, UInputAction inputAction)
    {
        Id = id;
        Text = Option<FText>.None;
        InputAction = inputAction;
    }
}