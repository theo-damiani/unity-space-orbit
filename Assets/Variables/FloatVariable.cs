using UnityEngine;

[CreateAssetMenu(fileName = "Variable", menuName = "Variables / Float")]
public class FloatVariable : BaseVariable<float>
{
    public override bool Clampable { get { return true; } }
    protected override float ClampValue(float value)
    {
        if (value.CompareTo(MinClampValue) < 0)
        {
            return MinClampValue;
        }
        else if (value.CompareTo(MaxClampValue) > 0)
        {
            return MaxClampValue;
        }
        else
        {
            return value;
        }
    }
}