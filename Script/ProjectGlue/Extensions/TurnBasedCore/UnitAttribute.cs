namespace UnrealSharp.TurnBasedCore;

public partial struct FBoolUnitAttribute
{
    public FBoolUnitAttribute(bool value)
    {
        BaseValue = value;
        CurrentValue = value;
    }
}

public partial struct FIntUnitAttribute
{
    public FIntUnitAttribute(int value)
    {
        BaseValue = value;
        CurrentValue = value;
    }
}

public partial struct FFloatUnitAttribute
{
    public FFloatUnitAttribute(float value)
    {
        BaseValue = value;
        CurrentValue = value;
    }
}