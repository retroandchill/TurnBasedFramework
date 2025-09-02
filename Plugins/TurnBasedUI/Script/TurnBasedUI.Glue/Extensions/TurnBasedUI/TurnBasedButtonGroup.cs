namespace UnrealSharp.TurnBasedUI;

public partial class UTurnBasedButtonGroup
{
    public event AddButton OnAddButton
    {
        add => OnAddButtonDelegate += value;
        remove => OnAddButtonDelegate -= value;
    }

    public event RemoveButton OnRemoveButton
    {
        add => OnRemoveButtonDelegate += value;
        remove => OnRemoveButtonDelegate -= value;
    }

}