using UnrealSharp.UnrealSharpCommonUI;

namespace UnrealSharp.TurnBasedUI;

public readonly record struct SelectedOption(int Index, FName Id, UTurnBasedButtonBase Button);

public partial class UOptionSelectionWidget
{
    public async Task<SelectedOption> SelectOptionAsync(CancellationToken cancellationToken = default)
    {
        var (button, index) = await Buttons.SelectButtonAsync(cancellationToken);
        return new SelectedOption(index, Options[index].Id, (UTurnBasedButtonBase)button);
    }
}