using System;
using UnityEngine;

[Serializable]
public class Vector3Reference
{
    public bool UseConstant = true;
    public Vector3 ConstantValue;
    public Vector3Variable Variable;

    public Vector3Reference()
    { }

    public Vector3Reference(Vector3 value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public Vector3 Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (UseConstant) {ConstantValue = value;} else {Variable.Value = value;}}
    }

    public GameEvent OnUpdateEvent
    {
        get { return UseConstant ? null : Variable.OnUpdateEvent;}
    }
}
