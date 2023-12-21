using System;

[Serializable]
public class BoolReference
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BoolVariable Variable;

    public BoolReference()
    { }

    public BoolReference(bool value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (UseConstant) {ConstantValue = value;} else {Variable.Value = value;}}
    }

    public GameEvent OnUpdateEvent
    {
        get { return UseConstant ? null : Variable.OnUpdateEvent;}
    }
}
